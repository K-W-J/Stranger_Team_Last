using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.Players.Ability;
using _01_Works.HS.Code.Players.Ability.BloodSteel;
using _01_Works.HS.Code.Players.Ability.ReflexAttack;
using _01_Works.HS.Code.Players.States;
using UnityEngine;

namespace _01_Works.HS.Code.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        private Player _player;
        private EntityAnimator _entityAnimator;
        private EntityVFX _vfxCompo;
        private EntityAnimatorTrigger _animatorTrigger;
        private ReflexAttackAbility _reflexAttackAbility;
        private BloodSteelAbility _bloodSteelAbility;
        
        private int _comboCounter;
        
        private readonly int _comboCounterHash = Animator.StringToHash("COMBO_COUNTER");

        public bool IsCanReflectBullet => _reflexAttackAbility.IsActiveAbility;
        public bool IsCanBloodSteel => _bloodSteelAbility.IsActiveAbility;

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _vfxCompo = entity.GetCompo<EntityVFX>();
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
        }
        
        public void AfterInitialize()
        {
            AbilityCompo abilityCompo = _player.GetCompo<AbilityCompo>();
            _reflexAttackAbility = abilityCompo.GetAbility(AbilityType.ReflexAttack) as ReflexAttackAbility;
            _bloodSteelAbility = abilityCompo.GetAbility(AbilityType.BloodSteel) as BloodSteelAbility;
            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger;
            _player.InputSO.OnAttackPressed += HandleAttackEvent;
        }

        private void OnDestroy()
        {
            _animatorTrigger.OnAttackVFXTrigger -= HandleAttackVFXTrigger; 
            _player.InputSO.OnAttackPressed -= HandleAttackEvent;
        }

        private void HandleAttackVFXTrigger()
        {
            _vfxCompo.PlayVFX($"Attack{_comboCounter}", Vector3.zero, Quaternion.identity);
        }
        
        private void HandleAttackEvent()
        {
            const string attack = "ATTACK";
            
            if (_player.GetCurrentState() is ICanAttackState)
                _player.ChangeState(attack);
        }

        public void Attack()
        {
            _comboCounter++;
            AudioManager.Instance.PlaySfx("ATTACK");
            _entityAnimator.SetParam(_comboCounterHash, _comboCounter);
        }

        public void EndAttack()
        {
            _comboCounter = 0;
            _entityAnimator.SetParam(_comboCounterHash, _comboCounter);
        }
    }
}