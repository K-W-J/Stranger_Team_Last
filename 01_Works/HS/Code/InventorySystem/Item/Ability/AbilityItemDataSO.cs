using _01_Works.HS.Code.Players.Ability;
using UnityEngine;

namespace _01_Works.HS.Code.InventorySystem.Item.Ability
{
    [CreateAssetMenu(fileName = "AbilityItemDataSO", menuName = "SO/Inventory/AbilityItemDataSO", order = 0)]
    public class AbilityItemDataSO : ItemDataSO
    {
        public AbilityType abilityType;
    }
}