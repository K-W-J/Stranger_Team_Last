using System;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.InventorySystem
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Image slotImage; 
        [SerializeField] private Color placedColor;

        private Color _defaultSlotColor;

        private bool _isPlace;

        public bool IsPlaced
        {
            get => _isPlace;
            set
            {
                _isPlace = value;
                slotImage.color = value ? placedColor : _defaultSlotColor;
            }
        }

        private void Awake()
        {
            _defaultSlotColor = slotImage.color;
        }
    }
}