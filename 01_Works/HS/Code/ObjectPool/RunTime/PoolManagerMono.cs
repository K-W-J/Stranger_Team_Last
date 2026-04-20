using _01_Works.JY._01_Scripts.Dependencies;
using UnityEngine;

namespace _01_Works.HS.Code.ObjectPool.RunTime
{
    [Provide]
    public class PoolManagerMono : MonoSingleton<PoolManagerMono>, IDependencyProvider
    {
        [SerializeField] private PoolManagerSO poolManager;

        private void Awake()
        {
            poolManager.Initialize(transform);
        }
        
        public T Pop<T>(PoolItemSO item) where T : IPoolable
        {
            return (T)poolManager.Pop(item);
        }
        
        public void Push(IPoolable item)
        {
            poolManager.Push(item);
        }
    }
}