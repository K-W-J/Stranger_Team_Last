using System.Collections.Generic;
using UnityEngine;

namespace _01_Works.HS.Code.InventorySystem.Item
{
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "SO/Inventory/ItemDataSO", order = 0)]
    public class ItemDataSO : ScriptableObject
    {
        [TextArea(1, 2)]
        public string itemName;
        [TextArea(3, 10)]
        public string itemDescription;
        
        public int price;
        public bool isHealthItem;
        
        public ItemBrick itemBrickPrefab;
        
        public int xSize;
        public int ySize;
        
        public List<Vector2Int> blockList =  new List<Vector2Int>();
    }
}