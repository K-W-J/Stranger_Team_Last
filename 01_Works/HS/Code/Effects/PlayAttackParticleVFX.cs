using System;
using System.Collections.Generic;
using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.HS.Code.Players;
using _01_Works.HS.Code.Players.Stat;
using _01_Works.JY._01_Scripts.Dependencies;
using _01_Works.KHJ;
using _01_Works.KHJ.Bullet;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Works.HS.Code.Effects
{
    public class PlayAttackParticleVFX : PlayParticleVFX
    {
        [SerializeField] private StatDataSO attackRangeStat;
        [SerializeField] private Player player;
        [SerializeField] private float attackEffectDuration;
        [SerializeField] private ParticleSystem[] scalingEffectList;
        [SerializeField] private PlayerAttackCompo attackCompo;
        [SerializeField] private float playDuration = 0.5f;
        
        [SerializeField] private GameEventChannelSO abilityChannel;
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private PoolItemSO hitImpactItem;
        
        [Inject] private PoolManagerMono _poolManager;
        
        private List<GameObject> _attackedObjList = new List<GameObject>();
        private Collider _collider;
        private float _originalScale;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _originalScale = transform.localScale.x;
            attackRangeStat.OnChangeValue += HandleChangeAttackRange;
        }

        private void OnDestroy()
        {
            attackRangeStat.OnChangeValue -= HandleChangeAttackRange;
        }

        private void HandleChangeAttackRange(float range)
        {
            transform.localScale = Vector3.one * (_originalScale * range);
            
            foreach (ParticleSystem particle  in scalingEffectList)
                particle.transform.localScale = Vector3.one * (_originalScale * range);
        }

        public override async void PlayVFX(Vector3 position, Quaternion rotation)
        {
            try
            {
                base.PlayVFX(position, rotation);

                // 참격 공격
                if (VFXName == "Attack1")
                    abilityChannel.RaiseEvent(AbilityEvents.SlashAttack.Initializer(
                        transform.rotation, transform.position));
                
                _attackedObjList.Clear();
                _collider.enabled = true;
                await Awaitable.WaitForSecondsAsync(attackEffectDuration);
                _collider.enabled = false;
            }
            catch (Exception e)
            {
                Debug.Log($"{e} : 이펙트 사라사라사라짐");
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            Vector3 directionToTarget = collision.transform.position - player.transform.position;
            float dotProduct = Vector3.Angle(player.transform.forward, directionToTarget);

            if (dotProduct > 110) return;
            // 앞에 있을 때
            
            if (collision.TryGetComponent(out IDamageable iDamageable) && _attackedObjList.Contains(collision.gameObject) == false)
            {
                Debug.Log($"{collision.gameObject.name}이 앞에 있음");
                iDamageable.ApplyDamage(10, player); 
                _attackedObjList.Add(collision.gameObject);
                
                ImpulseEvent impulseEvent = CameraEvents.ImpulseEvent.Initializer(0.1f);
                cameraChannel.RaiseEvent(impulseEvent);

                PlayEffect(collision.gameObject);
                
                if (attackCompo.IsCanBloodSteel)
                {
                    // if (Random.value <= 1f) // 100% 확률 
                    if (Random.value < 0.05f) // 1% 확률 
                    {
                        player.Heal();
                    }
                }
            }
            else if (attackCompo.IsCanReflectBullet && collision.TryGetComponent(out Bullet bullet))
            {
                Debug.Log($"튕겨냄");
                bullet.ReflectBullet(player.transform.forward);
            }
        }

        private async void PlayEffect(GameObject targetObj)
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(hitImpactItem);
            Vector3 effectPosition = targetObj.transform.position + new Vector3(
                Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)+1, Random.Range(-0.2f, 0.2f));
            effect.PlayVFX(effectPosition, Quaternion.identity); 
            
            await Awaitable.WaitForSecondsAsync(playDuration);
            _poolManager.Push(effect);
        }
    }
}