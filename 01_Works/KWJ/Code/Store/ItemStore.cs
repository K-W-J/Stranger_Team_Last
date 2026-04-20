using System;
using System.Collections.Generic;
using System.Linq;
using _01_Works.HS.Code.InventorySystem.Item;
using _01_Works.HS.Code.Players;
using Input;
using KWJ.Coins;
using UnityEngine;

namespace KWJ.Store
{
    public class ItemStore : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;
        
        [Space]
        
        [SerializeField] private List<ItemStand> _itemStands = new List<ItemStand>();
        [SerializeField] private GameObject _itemVisual;
        [SerializeField] private GameObject _itemCanvas;
            
        private ItemStand _selectedItem;
        public bool IsEnterPlayer { get; set; }
        private bool _isClosePlayer;

        public void Initialize()
        {
            IsEnterPlayer = true;
                
            foreach (var item in _itemStands)
            {
                if(ItemManager.Instance.TryGetRandomItem(out var itemData))
                { 
                    ItemPanel itemPanel = Instantiate(_itemCanvas, item.transform).GetComponent<ItemPanel>();
                    GameObject itemVisual = Instantiate(_itemVisual, item.transform);
                    
                    item.Initialize(itemData, itemVisual, itemPanel);
                    item.ShowItem();
                }
            }
        }
        private void OnEnable()
        {
            _playerInput.OnInteractPressed += OnInteract;
        }
        private void OnDisable()
        {
            _playerInput.OnInteractPressed -= OnInteract;
        }

        private void OnDestroy()
        {
            _playerInput.OnInteractPressed -= OnInteract;
        }

        private void OnInteract()   
        {
            if(!IsEnterPlayer) return;

            foreach (var item in _itemStands.Where(item => item.SphereOverlapChecker.ShereOverlapCheck()))
            {
                _selectedItem = item;
            }

            if (_selectedItem == null) return;

           if (_selectedItem.SphereOverlapChecker.ShereOverlapCheck()
               && CoinManager.Instance.HasEnoughCoins(_selectedItem.ItemPrice))
           {
                ItemManager.Instance.CreateItemBrick(_selectedItem.CurrentItem);
                _selectedItem.DestroyItem();
           }
        }
    }
}
