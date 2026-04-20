using System.Collections;
using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using DG.Tweening;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability.SpinSword
{
    public class SpinSword : MonoBehaviour
    {
        [SerializeField] private float attackTime = 5;
        [SerializeField] private float attackMoveSpeed = 5;
        [SerializeField] private float normalMoveSpeed = 3;
        [SerializeField] private float attackRotationSpeed = 3.5f;
        [SerializeField] private float normalRotationSpeed = 1.5f;
        [SerializeField] private float waitingTime = 2.5f;
        
        private SpinSwordAbility _spinSwordAbility;
        private Entity _entity;
        private Transform _targetTrm;
        
        private float _attackTimer;
        private bool _isAttacking;
        private bool _isWaiting;
        
        public void SetupSword(Entity entity, SpinSwordAbility spinSwordAbility)
        {
            _entity = entity;
            _targetTrm = entity.gameObject.transform;
            _spinSwordAbility = spinSwordAbility;
            _attackTimer = 0;
        }

        private void Update()
        {
            SwordMovement();
        }

        private void SwordMovement()
        {
            if (_isAttacking == false) 
                _attackTimer += Time.deltaTime;

            // 적이 만약에 있다면
            if (_attackTimer >= attackTime && _spinSwordAbility.TryGetClosestEnemyTrm(ref _targetTrm))
            {
                _attackTimer = 0;
                _isAttacking = true;
            }
            
            // 회전
            if (_isWaiting == false)
            {
                Vector3 moveDir = _targetTrm.position - transform.position;
                moveDir.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir),
                    Time.deltaTime * (_isAttacking ? attackRotationSpeed : normalRotationSpeed));
            }
            
            // 이동
            transform.position += transform.forward * ((_isAttacking ? attackMoveSpeed : normalMoveSpeed) * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (_isAttacking == false) return;
            
            if (collision.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.ApplyDamage(10, _entity);
                _targetTrm = _entity.gameObject.transform;
            }

            _targetTrm = _entity.gameObject.transform; // 임시
            _isAttacking = false;

            WaitMovement();
        }

        private async void WaitMovement()
        {
            _isWaiting = true;
            await Awaitable.WaitForSecondsAsync(waitingTime);
            _isWaiting = false;
        }
    }
}