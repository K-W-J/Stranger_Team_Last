using UnityEngine;
using UnityEngine.Events;

namespace KWJ.OverlapChecker
{
    public class OverlapChecker : MonoBehaviour
    {
        public UnityEvent OnTargetEnterEvent;
        
        [SerializeField] protected Transform m_checkPoint;
        [SerializeField] protected LayerMask m_targetMask;
        [SerializeField] protected int m_maxCount;
        
        protected Collider[] m_results;
        private void Awake()
        {
            m_results = new Collider[m_maxCount];
        }
    }
}