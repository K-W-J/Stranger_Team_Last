using _01_Works.HS.Code.InventorySystem.Item;
using _01_Works.HS.Code.UI;
using Input;
using UnityEngine;

namespace _01_Works.HS.Code.InventorySystem
{
    public class InventoryActive : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputSO;
        [SerializeField] private CanvasGroup inventoryGroup;
        [SerializeField] private GameObject itemInfoPanel;
        [SerializeField] private RewardUI rewardPanel;

        [SerializeField] private float activeAlpha = 0.8f;
        [SerializeField] private float activeSpeed = 8f;

        private float _timer;
        private bool _isRewardInventory;
        
        public bool IsActiveInventory { get; private set; }

        private void Awake()
        {
            inputSO.OnActiveInventory += HandleActiveInventory;
        }

        private void OnDestroy()
        {
            inputSO.OnActiveInventory -= HandleActiveInventory;
        }
        
        public void ActiveRewardUI(ItemBrick itemBrick)
        {
            itemBrick.transform.SetParent(rewardPanel.transform);
            itemBrick.transform.localPosition = Vector3.zero;
            
            itemInfoPanel.SetActive(false);
            rewardPanel.gameObject.SetActive(true);
            
            _isRewardInventory = true;
            IsActiveInventory = true;
            inventoryGroup.blocksRaycasts = true;
            rewardPanel.SetInfo(itemBrick.ItemDataSO);
            
            inputSO.IsActiveInventory = true;
        }

        public void DisableRewardUI()
        {
            itemInfoPanel.SetActive(true);
            rewardPanel.gameObject.SetActive(false);

            _isRewardInventory = false;
            IsActiveInventory = false;
            inventoryGroup.blocksRaycasts = IsActiveInventory;
            
            inputSO.IsActiveInventory = false;
        }

        public void HandleActiveInventory(bool isActive)
        {
            if (_isRewardInventory) return;
            
            itemInfoPanel.SetActive(isActive);
            rewardPanel.gameObject.SetActive(false);
            
            IsActiveInventory = isActive;
            inventoryGroup.blocksRaycasts = IsActiveInventory;
        }

        private void Update()
        {
            if (IsActiveInventory && _timer < 1)
                _timer += Time.deltaTime * activeSpeed;
            else if (!IsActiveInventory && _timer > 0)
                _timer -= Time.deltaTime * activeSpeed;
            
            inventoryGroup.alpha = activeAlpha * _timer;
        }
    }
}
