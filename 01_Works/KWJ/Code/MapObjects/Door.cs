using UnityEngine;

namespace KWJ.MapObjects
{
    public class Door : MonoBehaviour
    {
        public Transform Visual => _visual;
        [SerializeField] private Transform _visual;
        
        public Transform Collider => _collider;
        [SerializeField] private Transform _collider;
        
        public ParticleSystem Particle => _particle;
        [SerializeField] private ParticleSystem _particle;
    }
}