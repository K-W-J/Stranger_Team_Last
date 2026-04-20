using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability.SlashAttack
{
    public class Slash : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        
        [SerializeField] private int damage = 10;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float rotationSpeed = 5;
        [SerializeField] private float rotationStartTime = 0.1f;
        [SerializeField] private float lifeTime = 10;
        
        private Entity _entity;
        private Pool _myPool;
        private ParticleSystem _particle;
        private Rigidbody _rigidbody;

        private float _timer;
        private bool _isThrowing;
        private Quaternion _startRotation;
        
        public GameObject GameObject => gameObject;

        public void ThrowSlash(Vector3 position, Quaternion rotation, Vector3 direction, Entity entity)
        {
            transform.position = position;
            transform.rotation = rotation; 
            _rigidbody.linearVelocity = direction * moveSpeed;
            _entity = entity;
            
            _particle.Play();
            _isThrowing = true;
        }

        private void Update()
        {
            if (_isThrowing == false) return;
            
            Timer(); // 타이머 재는거
            RotateAndMoveSlash(); // 참격 회전시키는거
            CheckLifeTime(); // 라이프타임 확인하는거
        }

        private void Timer()
        {
            if (_timer < lifeTime)
                _timer += Time.deltaTime;
        }

        private void CheckLifeTime()
        {
            if (_timer > lifeTime)
            {
                _myPool.Push(this);
            }
        }

        private void RotateAndMoveSlash()
        {
            if (_timer > rotationStartTime)
            {
                _particle.Pause();
                Vector3 eulerAngle = new Vector3(0, rotationSpeed * 10, 0);
                
                transform.Rotate(eulerAngle * Time.deltaTime, Space.Self);
                transform.position += -transform.right * (Time.deltaTime * 1.3f);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (_isThrowing == false) return;
            
            if (collision.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.ApplyDamage(damage, _entity); 
            }
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _particle = GetComponent<ParticleSystem>();
            _rigidbody = GetComponent<Rigidbody>();
            
            _startRotation = transform.rotation;
        }

        public void ResetItem()
        {
            _timer = 0;
            _isThrowing = false;
            _rigidbody.linearVelocity = Vector3.zero;
            transform.rotation = _startRotation;
        }
    }
}