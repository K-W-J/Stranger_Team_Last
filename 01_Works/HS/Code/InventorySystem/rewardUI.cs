using _01_Works.HS.Code.InventorySystem.Item;
using TMPro;
using UnityEngine;

namespace _01_Works.HS.Code.UI
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameTMP;
        [SerializeField] private TextMeshProUGUI itemDescriptionTMP;

        public void SetInfo(ItemDataSO itemData)
        {
            itemNameTMP.text = itemData.itemName;
            itemDescriptionTMP.text = itemData.itemDescription;
        }
    }
}