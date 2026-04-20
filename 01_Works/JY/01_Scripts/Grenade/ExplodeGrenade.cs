using System;
using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Works.JY._01_Scripts.Grenade
{
    public class ExplodeGrenade : MonoBehaviour, IPoolable
    {
        [Header("Effects")]
        [SerializeField] private PoolItemSO explodeEffect;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private GameObject explosionIndicatorPrefab; // 폭발 범위 표시 프리팹

        [Header("Physics")]
        [SerializeField] private float groundDrag = 2f;
        [SerializeField] private float groundAngularDrag = 5f;

        [Header("Explosion")]
        [SerializeField] private float explosionRadius = 2f;
        [SerializeField] private LayerMask damageLayer;
        //[SerializeField] private int maxTargets = 1;

        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;

        private Rigidbody _rigid;
        private Pool _pool;
        private Collider[] _colliders;
        private GameObject _indicatorInstance; // 생성된 범위 표시 오브젝트 인스턴스
        private bool _hasLanded = false; // 착지 여부 확인

        private float _originalDrag;
        private float _originalAngularDrag;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _originalDrag = _rigid.linearDamping;
            _originalAngularDrag = _rigid.angularDamping;
            _colliders = new Collider[10];
        }

        private void Update()
        {
            if (_indicatorInstance != null)
                _indicatorInstance.transform.position = transform.position;
        }

        public void Throw(float power)
        {
            _hasLanded = false;
            _rigid.AddForce(transform.forward * power, ForceMode.Impulse);
            _rigid.AddTorque(Random.insideUnitSphere * Random.Range(1f, 5f), ForceMode.Impulse);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_hasLanded) return;
            _hasLanded = true;

            _rigid.linearDamping = groundDrag;
            _rigid.angularDamping = groundAngularDrag;
            if (explosionIndicatorPrefab)
            {
                _indicatorInstance = Instantiate(explosionIndicatorPrefab, transform.position, Quaternion.identity);
            }

            Explode();
        }

        private async void Explode()
        {
            try
            {
                if (_indicatorInstance != null)
                {
                    var indicatorRenderer = _indicatorInstance.GetComponent<MeshRenderer>();
                    if (indicatorRenderer != null)
                    {
                        float blinkDuration = 2.0f;
                        float elapsedTime = 0f;
                        float startInterval = 0.4f;
                        float endInterval = 0.05f;

                        while (elapsedTime < blinkDuration)
                        {
                            if(indicatorRenderer == null) break;
                            indicatorRenderer.enabled = !indicatorRenderer.enabled;

                            float progress = elapsedTime / blinkDuration;
                            float currentInterval = Mathf.Lerp(startInterval, endInterval, progress);

                            if (elapsedTime + currentInterval > blinkDuration)
                                currentInterval = blinkDuration - elapsedTime;
                        
                            await Awaitable.WaitForSecondsAsync(currentInterval);
                            elapsedTime += currentInterval;
                        }
                
                        if(_indicatorInstance != null)
                            Destroy(_indicatorInstance.gameObject);
                    }
                }
                else
                    await Awaitable.WaitForSecondsAsync(2f);

                var effectEvt = EffectEvents.PlayPoolEffect.Initializer(transform.position, Quaternion.identity, explodeEffect, 4f);
                effectChannel.RaiseEvent(effectEvt);
            
                int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _colliders, damageLayer);

                for (int i = 0; i < hitCount; i++)
                {
                    if (_colliders[i].TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.ApplyDamage(1, null);
                    }
                }

                _pool.Push(this);
            }
            catch (System.Exception)
            {
                Debug.Log("Exception");
                if(_indicatorInstance != null)
                    Destroy(_indicatorInstance.gameObject);
            }
        }

        public void ResetItem()
        {
            _rigid.linearDamping = _originalDrag;
            _rigid.angularDamping = _originalAngularDrag;
            
            _rigid.linearVelocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;
            _hasLanded = false;

            if (_indicatorInstance)
            {
                Destroy(_indicatorInstance);
                _indicatorInstance = null;
            }
        }
    }
}