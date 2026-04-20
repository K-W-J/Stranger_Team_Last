using System;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.UI
{
    public class HealthCellUI : MonoBehaviour
    {
        [SerializeField] private Sprite fullHealthIcon;
        [SerializeField] private Sprite halfHealthIcon;
        [SerializeField] private Sprite emptyHealthIcon;

        private Image _healthIcon;

        private void Awake()
        {
            _healthIcon = GetComponent<Image>();
        }

        public void SetHealth(int health)
        {
            switch (health)
            {
                case 0:
                    _healthIcon.sprite = emptyHealthIcon;
                    break;
                case 1:
                    _healthIcon.sprite = halfHealthIcon;
                    break;
                case 2:
                    _healthIcon.sprite = fullHealthIcon;
                    break;
            }
        }
    }
}