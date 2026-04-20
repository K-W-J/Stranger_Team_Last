using UnityEngine;

namespace KWJ.OverlapChecker
{
    public class SphereOverlapChecker : OverlapChecker
    {
        [SerializeField] private float _radius;

        public bool ShereOverlapCheck()
        {
            bool isOverlap = Physics.CheckSphere(m_checkPoint.position,
                _radius, m_targetMask);
            
            if (isOverlap)
            {
                OnTargetEnterEvent.Invoke();
            }
            
            return isOverlap;
        }

        public GameObject[] GetOverlapData()
        {
            int count = Physics.OverlapSphereNonAlloc(m_checkPoint.position, _radius,
                m_results, m_targetMask);
            
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
            Gizmos.DrawWireSphere(m_checkPoint.position, _radius);
        }
        #endif
    }
}