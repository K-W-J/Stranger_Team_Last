using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _01_Works.JY._01_Scripts.Dependencies
{
    [DefaultExecutionOrder(-10)] //0이 일반 스크립트
    public class Injector : MonoBehaviour
    {
        private const BindingFlags _BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static Injector _instance;
        
        private readonly Dictionary<Type, object> _registry = new Dictionary<Type, object>();

        private void Awake()
        {
            _instance = this;
            IEnumerable<IDependencyProvider> providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (IDependencyProvider pro in providers)
            {
                RegisterProvider(pro);
            }
            
            IEnumerable<MonoBehaviour> injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var mono in injectables)
            {
                Inject(mono);
            }
        }

        private void Inject(MonoBehaviour mono)
        {
            Type type = mono.GetType();
            
            IEnumerable<FieldInfo> injectableFields = type.GetFields(_BindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));

            foreach (var field in injectableFields)
            {
                Type fieldType = field.FieldType;
                object instance = ResolveType(fieldType);
                Debug.Assert(instance != null, $"Inject instance not found in registry : {fieldType.Name}");
                
                field.SetValue(mono, instance);
            }
            
            IEnumerable<MethodInfo> injectableMethods = type.GetMethods(_BindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));

            foreach (var method in injectableMethods)
            {
                Type[] requireParam = method.GetParameters()
                    .Select(p => p.ParameterType).ToArray();
                object[] paramValues = requireParam.Select(ResolveType).ToArray();
                method.Invoke(mono, paramValues);
            }
            
        }

        private object ResolveType(Type type)
        {
            _registry.TryGetValue(type, out object instance);
            return instance;
        }

        private bool IsInjectable(MonoBehaviour mono)
        {
            MemberInfo[] members = mono.GetType().GetMembers(_BindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider pro)
        {
            if(Attribute.IsDefined(pro.GetType(), typeof(ProvideAttribute)))
            {
                _registry.Add(pro.GetType(), pro);
                return;
            }
            
            MethodInfo[] methods = pro.GetType().GetMethods(_BindingFlags);

            foreach (var method in methods)
            {
                if(!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                
                Type returnType = method.ReturnType;
                object returnInstance = method.Invoke(pro, null);
                Debug.Assert(returnInstance != null, $"Provide method return void {method.Name}");
                
                _registry.Add(returnType, returnInstance);
            }
        }

        private IEnumerable<MonoBehaviour> FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
        
        public static void InjectTo(object target)
        {
            _instance?.InjectInternal(target);
        }
        
        private void InjectInternal(object obj)
        {
            Type type = obj.GetType();
            var fields = type.GetFields(_BindingFlags).Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));
            foreach (var field in fields)
            {
                var value = ResolveType(field.FieldType);
                Debug.Assert(value != null, $"Inject instance not found in registry : {field.FieldType.Name}");
                field.SetValue(obj, value);
            }

            var methods = type.GetMethods(_BindingFlags).Where(m => Attribute.IsDefined(m, typeof(InjectAttribute)));
            foreach (var method in methods)
            {
                var parameters = method.GetParameters().Select(p => ResolveType(p.ParameterType)).ToArray();
                method.Invoke(obj, parameters);
            }
        }
    }
}