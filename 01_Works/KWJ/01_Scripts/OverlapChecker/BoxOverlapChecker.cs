using UnityEngine;

namespace KWJ.OverlapChecker
{
    public class BoxOverlapChecker : OverlapChecker
    {
        [SerializeField] private Vector3 _boxSize;

        public bool BoxOverlapCheck()
        {
            bool isOverlap = Physics.CheckBox(m_checkPoint.position,
                _boxSize * 0.5f, m_checkPoint.rotation, m_targetMask);
            
            if (isOverlap)
            {
                OnTargetEnterEvent.Invoke();
            }
            
            return isOverlap;
        }
        
        public GameObject[] GetOverlapData()
        {
            int count = Physics.OverlapBoxNonAlloc(m_checkPoint.position, _boxSize * 0.5f,
                m_results, m_checkPoint.rotation, m_targetMask);
            
            GameObject[] targets = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                targets[i] = m_results[i].gameObject;
            }

            return targets;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(m_checkPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.matrix = m_checkPoint.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _boxSize);
        }
        #endif
    }
}