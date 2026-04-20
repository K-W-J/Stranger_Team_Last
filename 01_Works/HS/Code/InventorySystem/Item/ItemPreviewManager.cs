using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.InventorySystem.Item
{
    public class ItemPreviewManager : MonoSingleton<ItemPreviewManager>
    {
        [SerializeField] private Image itemPreviewImage;
        
        private bool _isShowingPreview;
        private Material _previewMaterial;
        private ItemBrick _targetItem;

        private readonly int _isPreviewHash = Shader.PropertyToID("_IsPreview");
        private readonly int _isCanPlaceHash = Shader.PropertyToID("_IsCanPlace");

        private void Awake()
        {
            // 메테리얼 세팅
            _previewMaterial = new Material(itemPreviewImage.material);
            itemPreviewImage.material = _previewMaterial;
            _previewMaterial.SetInt(_isPreviewHash, 1);
            _previewMaterial.SetInt(_isCanPlaceHash, 0);
        }

        public void StartShowPreviewItem(ItemBrick itemBrick)
        {
            Debug.Log("Start");
            itemPreviewImage.gameObject.SetActive(true);
            _targetItem = itemBrick;
            
            // 이미지 바꿔치기
            itemPreviewImage.sprite = _targetItem.Image.sprite;
            itemPreviewImage.rectTransform.sizeDelta = _targetItem.Image.rectTransform.sizeDelta;
            itemPreviewImage.rectTransform.rotation = _targetItem.Image.rectTransform.rotation;
            itemPreviewImage.rectTransform.pivot = _targetItem.Image.rectTransform.pivot;
        }

        public void MovePreviewItem()
        {
            Vector2 leftBottomPos = _targetItem.GetLeftBottomPos();
            Vector2 slotPos = Inventory.Instance.GetSlotCellPos(leftBottomPos, _targetItem);
            itemPreviewImage.transform.position = slotPos;
        }

        public void SetPreviewState(PlaceCheckerType checkerType) // 어떻게 보여줄지 체크
        {
            if (checkerType == PlaceCheckerType.OutOfBounds)
            {
                itemPreviewImage.gameObject.SetActive(false);
            }
            else
            {
                itemPreviewImage.gameObject.SetActive(true);
                _previewMaterial.SetInt(_isCanPlaceHash, checkerType == PlaceCheckerType.CanPlace ? 1 : 0);
            }
        }

        public void StopShowPreviewItem()
        {
            itemPreviewImage.gameObject.SetActive(false);
        }
    }
}