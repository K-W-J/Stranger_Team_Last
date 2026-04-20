using System.Collections;
using UnityEngine;
using _01_Works.HS.Code.Combat;
using KWJ.OverlapChecker;
using UnityEngine.Events;

namespace KWJ.MapObjects.Trap
{
    public class Explosive : MonoBehaviour
    {
        public UnityEvent OnExplosionEvent;
        
        [SerializeField] private GameObject _visual;
        [SerializeField] private SphereOverlapChecker _sphereOverlap;
        [SerializeField] private ParticleSystem _particleSystem;
        
        [Space]
        
        [SerializeField] protected float m_explosionDelay;
        [SerializeField] private float _damageDelay;
        [SerializeField] private int _explosionDamage;
        
        private float _currentTime;
        
        protected bool m_isExplosion;

        protected async void ExplosionDelay()
        {
            m_isExplosion = true;
            
            await Awaitable.WaitForSecondsAsync(m_explosionDelay);

            StartCoroutine(Explosion());
        }

        protected IEnumerator Explosion()
        {
            OnExplosionEvent?.Invoke();
                
            m_isExplosion = true;
            
            _visual.SetActive(false);
            _particleSystem.Play();
            
            while (_currentTime < _damageDelay)
            {
                _currentTime += Time.deltaTime;
                yield return null;
            }
            
            ExplosionDamage();
            Destroy(gameObject, 0.2f);
        }
        
        protected void ExplosionDamage()
        {
            if (_sphereOverlap.ShereOverlapCheck())
            {
                GameObject[] targets = _sphereOverlap.GetOverlapData();

                foreach (var target in targets)
                {
                    if (target.TryGetComponent<IDamageable>(out var explosion))
                    {
                        explosion.ApplyDamage(_explosionDamage, null);
                    }
                }
            }
        }
        
        #if UNITY_EDITOR
        [ContextMenu("TestExplosion")]
        public void TestExplosion()
        {
            StartCoroutine(Explosion());
        }
        #endif
    }
}