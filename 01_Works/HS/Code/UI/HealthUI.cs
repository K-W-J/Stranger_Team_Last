using System;
using System.Collections.Generic;
using _01_Works.HS.Code.Events;
using UnityEngine;

namespace _01_Works.HS.Code.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private HealthCellUI healthCellPrefab;
        [SerializeField] private GameEventChannelSO uiEventChannel;
        
        private List<HealthCellUI> _cells;

        private void Awake()
        {
            _cells = new List<HealthCellUI>();
            uiEventChannel.AddListener<ChangeHealth>(HandleChangeHealth);
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<ChangeHealth>(HandleChangeHealth);
        }

        private void HandleChangeHealth(ChangeHealth evt)
        {
            int currentHp = evt.CurrentHealth;
            
            // 체력바 지우기
            ClearHealth();

            // 최대 체력이 짝수라는 가정
            for (int i = 0; i < evt.MaxHealth / 2; i++)
            {
                HealthCellUI cell = Instantiate(healthCellPrefab, transform);
                _cells.Add(cell);
                
                if (currentHp >= 2)
                    cell.SetHealth(2);
                else if (currentHp == 1)
                    cell.SetHealth(1);
                else
                    cell.SetHealth(0);
                
                currentHp -= 2;
            }
        }

        private void ClearHealth()
        {
            if (_cells == null)
                _cells = new List<HealthCellUI>();
            
            foreach (var cell in _cells)
            {
                Debug.Log(cell);
                if (cell.gameObject != null)
                    Destroy(cell.gameObject);
            }

            _cells.Clear();
        }
    }
}