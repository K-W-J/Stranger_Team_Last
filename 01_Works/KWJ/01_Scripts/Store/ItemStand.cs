using _01_Works.HS.Code.InventorySystem.Item;
using KWJ.Coins;
using KWJ.OverlapChecker;
using UnityEngine;

namespace KWJ.Store
{
    public class ItemStand : MonoBehaviour
    {
        [field: SerializeField] public SphereOverlapChecker SphereOverlapChecker { get; private set; }
        [SerializeField] private Transform _itemSpawnPoint;
        public ItemDataSO CurrentItem => _currentItem;
        private ItemDataSO _currentItem;
        
        private ItemPanel _itemPanel;
        private GameObject _itemVisual;
        
        public int ItemPrice => _currentItem.price;
        
        public void ShowItem()
        {
            if (_itemVisual == null)
            {
                Debug.LogWarning("_itemVisual가 없습니다.");
                return;
            }
            
            _itemVisual.SetActive(true);
        }
        
        public void HideItem()
        {
            if (_itemVisual == null)
            {
                Debug.LogWarning("_itemVisual가 없습니다.");
                return;
            }
            
            _itemVisual.SetActive(false);
        }

        public void DestroyItem()
        {
            HideItem();
            _currentItem = null;
            Destroy(_itemPanel.gameObject);
        }

        public void Initialize(ItemDataSO itemData, GameObject itemVisual, ItemPanel itemPanel)
        {
            _currentItem =  itemData;
            
            _itemVisual = itemVisual;
            _itemVisual.transform.position = _itemSpawnPoint.position;
            
            _itemPanel = itemPanel;
            _itemPanel.transform.position = _itemSpawnPoint.position + Vector3.up * 3;
            _itemPanel.SetPanelTexts(_currentItem.itemName, _currentItem.itemDescription, _currentItem.price);
        }
    }
}