using _01_Works.HS.Code.Players;

namespace _01_Works.HS.Code.InventorySystem.Item.Stat
{
    public class StatItemBrick : ItemBrick
    {
        private StatItemDataSO StatItemData => ItemDataSO as StatItemDataSO;
        
        public override void ApplyItemEffect(PlayerItemCompo itemCompo)
        {
            base.ApplyItemEffect(itemCompo);
            itemCompo.StatCompo.AddStatValue(StatItemData.statType, StatItemData.value);
        }

        public override void RemoveItemEffect(PlayerItemCompo itemCompo)
        {
            itemCompo.StatCompo.RemoveStatValue(StatItemData.statType, StatItemData.value);
        }
    }
}