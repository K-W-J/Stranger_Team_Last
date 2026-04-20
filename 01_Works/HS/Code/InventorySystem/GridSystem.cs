using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _01_Works.HS.Code.InventorySystem
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputSO;
        
        private GridLayoutGroup _gridGroup;
        private RectTransform GridRect => _gridGroup.transform as RectTransform;

        private void Awake()
        {
            _gridGroup = GetComponent<GridLayoutGroup>();
        }

        public bool TryGetCellIndex(Vector3 screenPos, out Vector2Int index)
        {
            // 그리드 기준 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GridRect, screenPos, null, out Vector2 localPoint);

            // Grid 사이즈 만큼 나누고 반올림 보정
            index = new Vector2Int(
                Mathf.RoundToInt(localPoint.x / 120 - 0.5f),
                Mathf.RoundToInt(localPoint.y / 120 - 0.5f));

            const int xCnt = 8;
            const int yCnt = 6;

            // Grid안에 있는지
            return index.x >= 0 && index.x < xCnt && index.y >= 0 && index.y < yCnt;
        }
    }
}