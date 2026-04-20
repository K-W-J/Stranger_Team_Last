using System;

namespace _01_Works.JY._01_Scripts.Dependencies
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    { }
}