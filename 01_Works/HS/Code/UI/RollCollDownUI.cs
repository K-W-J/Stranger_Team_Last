using System;
using System.Collections.Generic;
using System.Linq;
using _01_Works.HS.Code.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.UI
{
    public class RollCollDownUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TextMeshProUGUI timerText;
        
        private List<Material> _materials = new List<Material>();
        
        private readonly int _gaugeValueHash = Shader.PropertyToID("_GaugeValue");

        private void Awake()
        {
            _materials = GetComponentsInChildren<Image>().Select(image => image.material).ToList();
            
            uiChannel.AddListener<ChangeRollCollDown>(HandleChangeCollDown);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<ChangeRollCollDown>(HandleChangeCollDown);
        }

        private void HandleChangeCollDown(ChangeRollCollDown evt)
        {
            var value = evt.CurTimer / evt.CoolDown;
            foreach (var mat in _materials)
                mat.SetFloat(_gaugeValueHash, (value == 0 ? - 0.01f : value));

            if (value <= 0 || value >= 1)
            {
                timerText.text = String.Empty;
            }
            else
            {
                var truncatedValue = Mathf.Floor((evt.CoolDown - evt.CurTimer) * 10f) / 10f;
                timerText.text = $"{truncatedValue}";
            }
        }
    }
}
