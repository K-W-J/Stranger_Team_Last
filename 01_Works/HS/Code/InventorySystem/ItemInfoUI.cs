using System;
using System.Collections.Generic;
using _01_Works.HS.Code.InventorySystem.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.UI
{
    public class ItemInfoUI : MonoBehaviour
    {
        [SerializeField] private Image itemInfoImage;
        [SerializeField] private TextMeshProUGUI itemNameTMP;
        [SerializeField] private TextMeshProUGUI itemDescriptionTMP;
        [SerializeField] private GameObject itemBrickPreview;
        [SerializeField] private GameObject slotPrefab;

        [SerializeField] private int cellSize = 80;
        
        private List<GameObject> _slotList = new List<GameObject>();

        private void Awake()
        {
            StopShowInfo();
        }

        public void SetInfo(ItemBrick itemBrick)
        {
            const float defaultSize = 0.5f;

            itemInfoImage.gameObject.SetActive(true);
            
            // 기본적인 이미지 가져오기
            itemInfoImage.sprite = itemBrick.Image.sprite;
            itemInfoImage.rectTransform.sizeDelta = itemBrick.Image.rectTransform.sizeDelta;
            itemInfoImage.rectTransform.rotation = itemBrick.Image.rectTransform.rotation;
            itemInfoImage.rectTransform.pivot = itemBrick.Image.rectTransform.pivot;
            
            // 크기에 맞게 사이트 조정
            itemInfoImage.sprite = itemBrick.Image.sprite;
            itemInfoImage.rectTransform.localScale = Vector3.one * 
                (defaultSize + (4 - Mathf.Max(itemBrick.ItemDataSO.xSize, itemBrick.ItemDataSO.ySize)) * 0.25f);
            
            // 텍스트 데이터
            itemNameTMP.text = itemBrick.ItemDataSO.itemName;
            itemDescriptionTMP.text = itemBrick.ItemDataSO.itemDescription;
            
            // 블록 미리보기 보여주깅깅깅
            foreach (var slot in _slotList)
                Destroy(slot);
            _slotList.Clear(); // 이거 여기서 원래 안할거임임

            foreach (var pos in itemBrick.ItemDataSO.blockList)
            {
                GameObject slot = Instantiate(slotPrefab, itemBrickPreview.transform);
                ((RectTransform)slot.transform).anchoredPosition = pos * cellSize;
                
                ((RectTransform)itemBrickPreview.transform).anchoredPosition 
                    = new Vector2(itemBrick.ItemDataSO.xSize-2, itemBrick.ItemDataSO.ySize-2) * -cellSize/2;
                
                _slotList.Add(slot);
            }
        }

        public void StopShowInfo()
        {
            itemInfoImage.gameObject.SetActive(false);

            itemNameTMP.text = String.Empty;
            itemDescriptionTMP.text = String.Empty;
            
            foreach (var slot in _slotList)
                Destroy(slot);
            _slotList.Clear();
        }
    }
}