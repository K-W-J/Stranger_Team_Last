using System.Collections.Generic;
using _01_Works.HS.Code.InventorySystem.Item;
using _01_Works.HS.Code.InventorySystem.Item.Ability;
using _01_Works.HS.Code.InventorySystem.Item.Stat;
using _01_Works.HS.Code.Players;
using _01_Works.HS.Code.UI;
using UnityEngine;

namespace _01_Works.HS.Code.InventorySystem
{
    public class Inventory : MonoSingleton<Inventory>
    {
        [SerializeField] private Transform slotParent;
        [SerializeField] private Slot slotPrefab;
        [SerializeField] private PlayerItemCompo playerItemCompo;
        [SerializeField] private Transform itemParent;
        
        private GridSystem _gridSystem;
        public InventoryActive InventoryActive { get; private set; }
        public ItemInfoUI ItemInfoUI { get; private set; }
        
        private Slot[,] _slotArray;
        private List<ItemBrick> _itemBrickList;
            
        private const int XCnt = 8;
        private const int YCnt = 6;

        private void Awake()
        {
            _gridSystem = GetComponentInChildren<GridSystem>();
            InventoryActive = GetComponent<InventoryActive>();
            ItemInfoUI = GetComponentInChildren<ItemInfoUI>();
        }

        private void Start()
        {
            _slotArray = new Slot[YCnt, XCnt];
            _itemBrickList = new List<ItemBrick>();
            for (int i = 0; i < YCnt; i++)
                for (int j = 0; j < XCnt; j++)
                    _slotArray[i, j] = Instantiate(slotPrefab, slotParent);
        }

        /// <summary>
        /// 아이템 설치 가능 확인 함수
        /// </summary>
        public PlaceCheckerType CheckCanPlace(Vector2 leftBottomPos, List<Vector2Int> blockList)
        {
            if (_gridSystem.TryGetCellIndex(leftBottomPos, out var index))
            {
                foreach (Vector2Int blockPos in blockList)
                {
                    Vector2Int blockIndex = index + blockPos;
                    if (blockIndex.x is < 0 or >= XCnt) return PlaceCheckerType.OutOfBounds;
                    if (blockIndex.y is < 0 or >= YCnt) return PlaceCheckerType.OutOfBounds;
                    if (_slotArray[blockIndex.y, blockIndex.x].IsPlaced) return PlaceCheckerType.CanNotPlace;
                }
                return PlaceCheckerType.CanPlace;
            }

            return PlaceCheckerType.OutOfBounds;
        }

        /// <summary>
        /// 실질적 아이템 설치 함수,
        /// 설치 가능 여부 체크 안함
        /// </summary>
        public void PlaceItem(Vector2 leftBottomPos, ItemBrick itemBrick)
        {
            if (_gridSystem.TryGetCellIndex(leftBottomPos, out var leftBottomIndex))
            {
                PlaceItem(leftBottomIndex, itemBrick);
            }
        }
        
        public void PlaceItem(Vector2Int leftBottomIndex, ItemBrick itemBrick)
        {
            foreach (Vector2Int blockPos in itemBrick.ItemDataSO.blockList)
            {
                Vector2Int blockIndex = leftBottomIndex + blockPos;
                _slotArray[blockIndex.y, blockIndex.x].IsPlaced = true;
            }
            
            var itemPos = GetItemPlacePos(leftBottomIndex, itemBrick);
            itemBrick.transform.position = itemPos;
            itemBrick.ItemLeftBottom = leftBottomIndex;
            
            _itemBrickList.Add(itemBrick);
            if (itemBrick.IsPlaced == false)
            {
                InventoryActive.DisableRewardUI();
                itemBrick.ApplyItemEffect(playerItemCompo);
                itemBrick.transform.SetParent(itemParent);
            }
        }

        private Vector2 GetItemPlacePos(Vector2Int leftBottomIndex, ItemBrick itemBrick)
        {
            Vector2 itemPos = new Vector2(
                _slotArray[leftBottomIndex.y, leftBottomIndex.x].transform.position.x + (itemBrick.ItemDataSO.xSize - 1) * 60,
                _slotArray[leftBottomIndex.y, leftBottomIndex.x].transform.position.y + (itemBrick.ItemDataSO.ySize - 1) * 60);
            return itemPos;
        }

        /// <summary>
        /// 이거 걍 드래그 했을 때 인벤토리에서 잠깐 없애는거
        /// </summary>
        public void StartDragItem(ItemBrick itemBrick)
        {
            if (_itemBrickList.Contains(itemBrick) == false || itemBrick.IsPlaced == false)
            {
                Debug.Log("인벤토리에 없던 애가 지워졌는데?");
                return;
            }
            
            foreach (Vector2Int blockPos in itemBrick.ItemDataSO.blockList)
            {
                Vector2Int blockIndex = itemBrick.ItemLeftBottom + blockPos;
                _slotArray[blockIndex.y, blockIndex.x].IsPlaced = false;
            }

            _itemBrickList.Remove(itemBrick);
        }

        public Vector2 GetSlotCellPos(Vector2 leftBottomPos, ItemBrick itemBrick)
        {
            if (_gridSystem.TryGetCellIndex(leftBottomPos, out var leftBottomIndex))
            {
                return GetItemPlacePos(leftBottomIndex, itemBrick);
            }

            return Vector2.zero;
        }

        public bool RemoveItem(ItemBrick itemBrick)
        {
            if (itemBrick.IsPlaced == false) // 방금 얻은 걸 지웠을 때
            {
                InventoryActive.DisableRewardUI();
            }
            else // 이미 있던 걸 지웠을 때
            {
                if (itemBrick.ItemDataSO.isHealthItem) // 체력 템 빼면 죽는지 체크
                {
                    var healthCompo = playerItemCompo.Player.HealthCompo;
                    if (itemBrick.ItemDataSO is StatItemDataSO 
                        && healthCompo.CurrentHealth <= 2)
                        return false;
                    if (itemBrick.ItemDataSO is AbilityItemDataSO 
                        && healthCompo.CurrentHealth <= 4)
                        return false;
                }
                itemBrick.RemoveItemEffect(playerItemCompo);
            }
            
            ItemManager.Instance.SubtractCount(itemBrick.ItemDataSO);

            return true;
        }
    }
}
