using System.Collections.Generic;
using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Stat
{
    public class StatCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private List<StatDataSO> statDataList; 
        private Dictionary<StatType, StatDataSO> _statDataDict = new Dictionary<StatType, StatDataSO>();
            
        public void Initialize(Entity entity)
        {
            foreach (StatDataSO statData in statDataList)
                _statDataDict.Add(statData.statType, statData);
        }

        public StatDataSO GetStatData(StatType type) => _statDataDict[type];
        public float GetStatValue(StatType statType) => _statDataDict[statType].currentValue;
        public void AddStatValue(StatType statType, float value) => _statDataDict[statType].AddValue(value);
        public void RemoveStatValue(StatType statType, float value) => _statDataDict[statType].RemoveValue(value);
    }
}
