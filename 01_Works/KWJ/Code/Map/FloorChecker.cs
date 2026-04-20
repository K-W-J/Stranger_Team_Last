using UnityEngine;

namespace KWJ.Map
{
    public class FloorChecker : MonoBehaviour
    {
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private float distance;
        [SerializeField] private LayerMask floorMask;
        
        [SerializeField] private Transform[] wallPoints;

        public bool FloorCheck(Transform wallTrm)
        {
            return Physics.CheckBox(wallTrm.position + (wallTrm.localPosition * distance)
                , boxSize, Quaternion.identity, floorMask);
        }

        private void OnDrawGizmos()
        {
            
            Gizmos.color = Color.red;
            foreach (var wallPoint in wallPoints)
            {
                if(wallPoint == null) return;
                
                Gizmos.DrawWireCube(wallPoint.position + (wallPoint.localPosition * distance), boxSize * 2);
            }
        }
    }
}