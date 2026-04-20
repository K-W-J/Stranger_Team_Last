using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Effects;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.HS.Code.Players;
using _01_Works.JY._01_Scripts.Dependencies;
using _01_Works.JY.Combat;
using _01_Works.KHJ;
using _01_Works.KHJ.Skill;
using UnityEngine;
using static Unity.Cinemachine.CinemachineImpulseManager;
using ImpulseEvent = _01_Works.KHJ.ImpulseEvent;

namespace Blade.SkillSystem
{
    public class BombSkill : Skill
    {
        [SerializeField] private PoolManagerMono _poolManagerMono;
        [SerializeField] private PoolItemSO _bombEffect;

        [SerializeField] private Transform[] muzzles;
        [SerializeField] private Transform[] pivots;
        [SerializeField] private Transform _tank;

        [SerializeField] private RoundDecal decal;
        [SerializeField] private float chargeSpeed = 2f;
        [SerializeField] private float maxRadius = 3f;

        [SerializeField] private AttackDataSO bombAttackData;

        //1�б⶧ �������� �Ѱ����� ����ϵ��� ������Ʈ�� �������.

        [SerializeField] private PoolItemSO bombEffect;
        [Inject] private PoolManagerMono _poolManager;

        public bool IsCharging { get; set; }
        private float _currentRadius;

        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            decal.SetProjectorActive(false); //ó�� �����ϸ� ���ְ�.
        }

        public override void UseSkill()
        {
            base.UseSkill(); //��Ÿ�Ӱ���
            int enemyCount = _skillComponent.GetEnemiesInRange(decal.transform.position, _currentRadius);

            PoolingEffect effect2 = _poolManagerMono.Pop<PoolingEffect>(_bombEffect);
            Vector3 pos = _tank.position + muzzles[1].forward * UnityEngine.Random.Range(7, 35);
            pos.y = 15f;  // ���� �ٵ��� ���� ����

            effect2.PlayVFX(pos, Quaternion.Euler(90f, 0f, 0f));

            for (int i = 0; i < enemyCount; i++)
            {
                Collider target = _skillComponent.Colliders[i];
                PoolingEffect effect = _poolManager.Pop<PoolingEffect>(bombEffect);
                effect.PlayVFX(target.transform.position, Quaternion.identity);
                DelayedPooling(effect, 2f);
                if (target.transform.TryGetComponent(out Player player))
                {
                    Debug.Log("������ ����");
                }
            }

            if (enemyCount > 0)
            {
                ImpulseEvent evt = CameraEvents.ImpulseEvent.Initializer(bombAttackData.impulseForce);
                _skillComponent.CameraChannel.RaiseEvent(evt);
            }
        }

        private async void DelayedPooling(PoolingEffect effect, float duration)
        {
            await Awaitable.WaitForSecondsAsync(duration); //���� Awaitable ����
            _poolManager.Push(effect);
        }

        public void StartCharging()
        {
            _currentRadius = 0.1f;
            SetChargingStatus(true);
        }

        private void SetChargingStatus(bool isCharging)
        {
            decal.SetProjectorActive(isCharging);
            IsCharging = isCharging;
        }

        public void ReleaseCharging()
        {
            SetChargingStatus(false);
            UseSkill(); //��ų ���
        }

        public void CancelCharging()
        {
            SetChargingStatus(false);
        }

        protected override void Update()
        {
            base.Update();
            if (IsCharging)
            {
                _currentRadius += Time.deltaTime * chargeSpeed;
                _currentRadius = Mathf.Clamp(_currentRadius, 0, maxRadius);
                decal.SetDecalSize(_currentRadius);
            }
        }
    }
}