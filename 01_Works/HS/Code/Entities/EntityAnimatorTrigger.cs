using System;
using UnityEngine;

namespace _01_Works.HS.Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationStartTrigger;
        public Action OnAnimationEndTrigger;
        public Action<bool> OnRollingStatusChange;
        public Action<bool> OnAttackStatusChange;
        public Action OnAttackVFXTrigger;
        public Action<bool> OnManualRotationTrigger;
        public Action OnDamageCastTrigger;
        public Action<bool> OnDamageToggleTrigger;
        public Action OnAttackTrigger;
        public Action OnAnimationComboEnd;
        public Action OnEnemyGrenadeTrigger;
        public Action OnThirdAttackTrigger;
        
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationStart() => OnAnimationStartTrigger?.Invoke();
        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        private void RollingStart() => OnRollingStatusChange?.Invoke(true);
        private void RollingEnd() => OnRollingStatusChange?.Invoke(false);
        private void AttackingStart() => OnAttackStatusChange?.Invoke(true);
        private void AttackingEnd() => OnAttackStatusChange?.Invoke(false);
        private void AnimationComboEnd() => OnAnimationComboEnd?.Invoke();
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void StartManualRotation() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false);
        private void DamageCast() => OnDamageCastTrigger?.Invoke();
        private void StartDamageCast() => OnDamageToggleTrigger?.Invoke(true);
        private void StopDamageCast() => OnDamageToggleTrigger?.Invoke(false);
        private void EnemyAttack() => OnAttackTrigger?.Invoke();
        private void EnemyGrenade() => OnEnemyGrenadeTrigger?.Invoke();
        private void EnemyThirdAttack() => OnThirdAttackTrigger?.Invoke();
    }
}