using System.Collections.Generic;
using System.Linq;
using _01_Works.HS.Code.InventorySystem.Item.Ability;
using _01_Works.HS.Code.InventorySystem.Item.Stat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Works.HS.Code.InventorySystem.Item
{
    public class ItemManager : MonoSingleton<ItemManager>
    {
        [SerializeField] private int maxStatItemCount = 4;
        [Range(0, 1)] [SerializeField] private float statItemPercent;
        
        [SerializeField] private List<StatItemDataSO> statItemList;
        [SerializeField] private List<AbilityItemDataSO> abilityItemList;
        [SerializeField] private Transform itemParent;
        [SerializeField] private InventoryActive inventoryActive;
        
        private List<ItemBrick> _itemBrickList = new List<ItemBrick>();
        private Dictionary<ItemDataSO, int> _itemCountDict = new Dictionary<ItemDataSO, int>();

        private void Awake()
        {
            foreach (StatItemDataSO itemData in statItemList)
                _itemCountDict.Add(itemData, 0);
            foreach (AbilityItemDataSO itemData in abilityItemList)
                _itemCountDict.Add(itemData, 0);
        }

        public bool TryGetRandomItem(out ItemDataSO itemData)
        {
            bool isCreateStatItem = Random.value <= statItemPercent;
            var canSpawnStatList =
                _itemCountDict.Where(pair => pair.Key is StatItemDataSO && pair.Value < maxStatItemCount).ToList();
            var canSpawnAbilityList =
                _itemCountDict.Where(pair => pair.Key is AbilityItemDataSO && pair.Value < 1).ToList();
            
            if ((isCreateStatItem || canSpawnAbilityList.Count == 0) && canSpawnStatList.Count > 0)
            {
                itemData = canSpawnStatList[Random.Range(0, canSpawnStatList.Count)].Key;
            }
            else if ((!isCreateStatItem || canSpawnStatList.Count == 0) && canSpawnAbilityList.Count > 0)
            {
                itemData = canSpawnAbilityList[Random.Range(0, canSpawnAbilityList.Count)].Key;
            }
            else
                itemData = null;
            
            Debug.Assert(itemData != null, "아이템 못 만듦ㅠㅠ");
            
            return (itemData != null);
        }

        [ContextMenu("Create")]
        public void CreateRandomItemBrick()
        {
            if (TryGetRandomItem(out var itemData))
            {
                CreateItemBrick(itemData);
            }
        }

        public void CreateItemBrick(ItemDataSO itemData)
        {
            ItemBrick itemBrick = Instantiate(itemData.itemBrickPrefab, itemParent);
            _itemCountDict[itemData]++;
            Debug.Log(_itemCountDict[itemData]);
            _itemBrickList.Add(itemBrick);
            
            inventoryActive.ActiveRewardUI(itemBrick);
        }

        public void SubtractCount(ItemDataSO itemData) => _itemCountDict[itemData]--;
    }
}
