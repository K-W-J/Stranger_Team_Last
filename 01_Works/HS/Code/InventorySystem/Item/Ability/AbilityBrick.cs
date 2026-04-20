using _01_Works.HS.Code.Players;

namespace _01_Works.HS.Code.InventorySystem.Item.Ability
{
    public class AbilityBrick : ItemBrick
    {
        private AbilityItemDataSO AbilityItemData => ItemDataSO as AbilityItemDataSO;

        public override void ApplyItemEffect(PlayerItemCompo itemCompo)
        {
            base.ApplyItemEffect(itemCompo);
            itemCompo.AbilityCompo.ActiveAbility(AbilityItemData.abilityType, true);
        }

        public override void RemoveItemEffect(PlayerItemCompo itemCompo)
        {
            itemCompo.AbilityCompo.ActiveAbility(AbilityItemData.abilityType, false);
        }
    }
}