using System;
using _01_Works.HS.Code.InventorySystem.Item;
using DG.Tweening;
using Input;
using KWJ.OverlapChecker;
using UnityEngine;

namespace KWJ.MapObjects
{
    public class TreasureChest : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;
        
        [Space]
        
        [SerializeField] private SphereOverlapChecker _sphereChecker;
        [SerializeField] private Transform _chestLid;
        [SerializeField] private GameObject _panel;

        private bool _isOpen;

        private void Start()
        {
            _panel.SetActive(false);
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

        private void Update()
        {
            if (_sphereChecker.ShereOverlapCheck() && !_isOpen)
                _panel.SetActive(true);
            else
                _panel.SetActive(false);
        }

        private void OnInteract()   
        {
            if (_isOpen)
            {
                _playerInput.OnInteractPressed -= OnInteract;
                return;
            }

            if (_sphereChecker.ShereOverlapCheck())
            {
                ItemManager.Instance.CreateRandomItemBrick();
                _chestLid.DOLocalRotate(Vector3.left * 110, 3f);
                _panel.SetActive(false);
                _isOpen = true;
            }
        }
    }
}
