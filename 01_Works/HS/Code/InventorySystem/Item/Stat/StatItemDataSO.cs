using _01_Works.HS.Code.Players.Stat;
using UnityEngine;

namespace _01_Works.HS.Code.InventorySystem.Item.Stat
{
    [CreateAssetMenu(fileName = "StatItemDataSO", menuName = "SO/Inventory/StatItemDataSO", order = 0)]
    public class StatItemDataSO : ItemDataSO
    {
        public StatType statType;
        public float value;
    }
}