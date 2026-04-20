using System;
using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.HS.Code.Players;
using _01_Works.JY.Events;
using _01_Works.JY._01_Scripts.Bullet.SpecialBullet;
using _01_Works.JY.Enemies;
using KWJ.MapObjects.Trap;
using UnityEngine;

namespace _01_Works.KHJ.Bullet
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private LayerMask whatIsTargets;
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private PoolItemSO reflectPoolItem;
        [SerializeField] private PoolItemSO popEffectItem;
        [SerializeField] private GameEventChannelSO bulletChannel;
        [SerializeField] private GameEventChannelSO effectChannel;
        
        [SerializeField] private Material reflectMaterial;
        private Material originalMaterial;

        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;

        public float speed;
        public Entity enemy { get; set; }
        public bool IsReflect { get; set; }

        [SerializeField] private bool isSpecial;

        private Rigidbody _rigid;
        private float _originalSpeed;

        protected Pool _pool;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _originalSpeed = speed;
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            speed = _originalSpeed;
            IsReflect = false;
            _rigid.isKinematic = false;
        }

        public virtual void Fire()
        {
            _rigid.linearVelocity = transform.forward * speed;
        }

        public void ReflectBullet(Vector3 direction)
        {
            var bulletEvt = BulletEvents.SpawnBulletsInCircle.Initializer
                (transform.position, direction, reflectPoolItem, null, _originalSpeed - 2f, true);
            bulletChannel.RaiseEvent(bulletEvt);
            _pool.Push(this);
        }

        public async void BulletLifeTime(float poolDuration)
        {
            try
            {
                await Awaitable.WaitForSecondsAsync(poolDuration);
                if (this == null || !gameObject.activeInHierarchy) return;
                _pool.Push(this);
            }
            catch (System.Exception)
            {
                Debug.Log("Exception");
            }
        }

        private void BulletPop()
        {
            Vector3 Pos = new Vector3(transform.position.x, 1, transform.position.z);
            var popEffect = EffectEvents.PlayPoolEffect.Initializer(Pos, Quaternion.identity,popEffectItem, 3f);
            effectChannel.RaiseEvent(popEffect);
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if ((whatIsTargets.value & (1 << other.gameObject.layer)) > 0 && !isSpecial)
            {
                if (other.TryGetComponent<Player>(out Player p))
                {
                    if (p.IsCanHit)
                        BulletPop();
                }
                
                if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    if (other.TryGetComponent<Player>(out Player player))
                    {
                        damageable.ApplyDamage(1, null);
                    }
                    else if (other.TryGetComponent<Enemy>(out Enemy e))
                    {
                        if (IsReflect)
                        {
                            damageable.ApplyDamage(10, null);
                            BulletPop();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (other.TryGetComponent<Destructible>(out Destructible des) || other.TryGetComponent<Drum>(out Drum drum))
                    {
                        BulletPop();
                        damageable.ApplyDamage(1, null);
                    }
                }
                _pool.Push(this);
            }
        }
    }
}