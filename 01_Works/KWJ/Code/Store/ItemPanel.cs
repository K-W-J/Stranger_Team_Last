using TMPro;
using UnityEngine;

namespace KWJ.Store
{
    public class ItemPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _explanationText;
        [SerializeField] private TextMeshProUGUI _priceText;

        public void SetPanelTexts(string title, string explanation, int price)
        {
            _titleText.text = title;
            _explanationText.text = explanation;
            _priceText.text = $"[E] 구매하기 ({price})";
        }
    }
}