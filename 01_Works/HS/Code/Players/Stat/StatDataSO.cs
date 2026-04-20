using System;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Stat
{
    [CreateAssetMenu(fileName = "StatDataSO", menuName = "SO/StatDataSO", order = 0)]
    public class StatDataSO : ScriptableObject
    {
        public StatType statType;
		public float defaultValue;
        public float currentValue;

        public Action<float> OnChangeValue;

        private void OnEnable()
        {
            currentValue = defaultValue;
        }

        public void AddValue(float value)
        {
            currentValue += value;
            OnChangeValue?.Invoke(currentValue);
        }
        
        public void RemoveValue(float value)
        {
            currentValue -= value;
            OnChangeValue?.Invoke(currentValue);
        }
    }
}