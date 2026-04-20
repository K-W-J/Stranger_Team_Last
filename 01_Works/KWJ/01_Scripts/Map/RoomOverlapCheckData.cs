using UnityEngine;

namespace KWJ.Map
{
    public class RoomOverlapCheckData : MonoBehaviour
    {
        public Vector3 RoomCheckPos => transform.position;
        [field: SerializeField] public Vector3 BoxSize { get; private set; }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(RoomCheckPos, BoxSize);
        }
        
        #endif
    }
}