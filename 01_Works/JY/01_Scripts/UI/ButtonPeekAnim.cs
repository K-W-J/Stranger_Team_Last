using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace _01_Works.JY._01_Scripts.UI
{
    public class ButtonPeekAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform button;
        private Vector2 originPos;
        private const float PeekOffsetX = 100f;

        private void Awake()
        {
            originPos = button.anchoredPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            button.DOKill();
            button.DOAnchorPos(new Vector2(originPos.x + PeekOffsetX, originPos.y), 0.2f).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            button.DOKill();
            button.DOAnchorPos(originPos, 0.2f).SetUpdate(true);
        }
        
        public void OpenSetting()
        {
            SettingPanelToggle.Instance.OpenSettingPanel();
        }
    }
}
