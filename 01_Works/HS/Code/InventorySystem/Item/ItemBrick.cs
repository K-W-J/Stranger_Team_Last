using System.Collections;
using System.Collections.Generic;
using _01_Works.HS.Code.Players;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _01_Works.HS.Code.InventorySystem.Item
{
    public abstract class ItemBrick : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [field:SerializeField] public ItemDataSO ItemDataSO {get; private set;}

        private bool _isDragging;
        private bool _isHovering;
        private bool _isDestroy;
        
        private Vector2 _startPos; // 드래그 실패시 돌아오기 용
        private Tween _hoverTween;
        private Material _material;
        
        public Image Image { get; private set; }

        public bool IsPlaced { get; private set; } // 한 번이라도 배치가 된 적이 있는가
        public Vector2Int ItemLeftBottom { get; set; } = new Vector2Int(-1, -1);

        private readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
            
        protected virtual void Awake()
        {
            Image = GetComponent<Image>();
            _material = new Material(Image.material);
            Image.material = _material;
        }

        private void Update()
        {
            if (_isDestroy) return;
            
            Drag();
            Hover();
        }

        private void Hover()
        {
            if (_isHovering == false) return;
            if (!Inventory.Instance.InventoryActive.IsActiveInventory)
            {
                StopHover();
            }
        }

        private void Drag()
        {
            if (_isDragging == false) return;
            if (!Inventory.Instance.InventoryActive.IsActiveInventory) // 인벤 꺼지면 드래그 종료
            {
                StopDrag(null);
                return;
            }
                
            Vector2 curLeftBottomPos = GetLeftBottomPos();
            PlaceCheckerType checkerType = Inventory.Instance.CheckCanPlace(curLeftBottomPos, ItemDataSO.blockList);
                
            ItemPreviewManager.Instance.MovePreviewItem(); // 미리보기 보여주는 중중중
            ItemPreviewManager.Instance.SetPreviewState(checkerType); // 미리보기 보여주는 중중중
            
            transform.position = Mouse.current.position.ReadValue();
        }

        public void OnPointerDown(PointerEventData eventData) // 드래그 시작
        {
            if (_isDestroy) return;
            if (eventData.button != PointerEventData.InputButton.Left) return; // 오른쪽 클릭시 드래그 방지
            if (!Inventory.Instance.InventoryActive.IsActiveInventory) return;
            
            ItemPreviewManager.Instance.StartShowPreviewItem(this); // 미리보기 보여주기 시작
            _isDragging = true;
            _startPos = transform.position;
            transform.SetAsLastSibling();
            
            Inventory.Instance.StartDragItem(this);
        }

        public void OnPointerUp(PointerEventData eventData) // 드래그 끝
        {
            if (_isDestroy) return;
            if (eventData.button != PointerEventData.InputButton.Left) return; // 오른쪽 클릭시 드래그 방지
            if (!Inventory.Instance.InventoryActive.IsActiveInventory) return;

            StopDrag(eventData);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isDestroy) return;
            if (!Inventory.Instance.InventoryActive.IsActiveInventory) return;
            
            _isHovering = true;
            Inventory.Instance.ItemInfoUI.SetInfo(this);
            
            _hoverTween = transform.DOScale(Vector3.one * 1.08f, 0.75f)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDestroy) return;
            if (!Inventory.Instance.InventoryActive.IsActiveInventory) return;
            StopHover();
        }
        
        private void StopDrag(PointerEventData eventData)
        {
            if (_isDestroy) return;
            
            _isDragging = false;
            ItemPreviewManager.Instance.StopShowPreviewItem(); // 미리보기 보여주기 종료

            if (eventData != null)
                if (CheckRemoveItem(eventData))
                    return;
            
            PlaceItem();
        }

        private bool CheckRemoveItem(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("GarbageCan"))
                {
                    if (Inventory.Instance.RemoveItem(this) == false)
                        return false;
                    _isDestroy = true;
                    DOTween.Kill(_hoverTween);
                    DOTween.To(() => 0, x=>
                        _material.SetFloat(_dissolveAmount, x), 1f, 1f)
                        .OnComplete(() => Destroy(gameObject));
                    
                    return true;
                }
            }

            return false;
        }

        private void PlaceItem()
        {
            if (_isDestroy) return;
            
            // 아이템 가장 왼쪽 아래 위치 찾기
            var curLeftBottomPos = GetLeftBottomPos();

            // 설치가 가능하다면
            if (Inventory.Instance.CheckCanPlace(curLeftBottomPos, ItemDataSO.blockList) == PlaceCheckerType.CanPlace)
            {
                Inventory.Instance.PlaceItem(curLeftBottomPos, this);
            }
            else // 실패하면 돌려두기
            {
                if (ItemLeftBottom != new Vector2Int(-1, -1)) // 돌아갈 곳이 있다면
                {
                    Inventory.Instance.PlaceItem(ItemLeftBottom, this);
                }
                
                // 있든 없든 일단 전 위치로 보내
                transform.position = _startPos;
            }
        }

        private void StopHover()
        {
            _isHovering = false;
            Inventory.Instance.ItemInfoUI.StopShowInfo();
            
            _hoverTween.Kill();
            transform.localScale = Vector3.one;
        }
        
        public Vector2 GetLeftBottomPos()
        {
            const int halfCellSize = 60;
            
            Vector2 curLeftBottomPos = new Vector2(
                transform.position.x - (ItemDataSO.xSize - 1) * halfCellSize,
                transform.position.y - (ItemDataSO.ySize - 1) * halfCellSize);
            return curLeftBottomPos;
        }

        public virtual void ApplyItemEffect(PlayerItemCompo itemCompo) => IsPlaced = true;
        public virtual void RemoveItemEffect(PlayerItemCompo itemCompo) => IsPlaced = false;
    }
}
