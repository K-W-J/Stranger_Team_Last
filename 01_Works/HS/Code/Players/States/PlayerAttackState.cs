using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.States
{
    public class PlayerAttackState : PlayerState, ICanRollState
    {
        private PlayerAttackCompo _attackCompo;
        private PlayerAimController _aimController;
        private float _moveSpeed;
        private float _rotateSpeed;

        private bool _isCanNextAttack;
        private bool _isPressed;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
            _aimController = entity.GetCompo<PlayerAimController>();
        }

        public override void Enter()
        {
            base.Enter();
            _isPressed = false;
            _isCanNextAttack = false;
            
            _moveSpeed = 0.6f;
            _rotateSpeed = _aimController.rotationSpeed;
            _aimController.rotationSpeed = 1.3f;
                
            _player.InputSO.OnAttackPressed += HandleAttackPressed;
            _animatorTrigger.OnAttackStatusChange += HandleAttackStatusChange;
            _animatorTrigger.OnAnimationComboEnd += HandleAnimationComboEnd; 
        }
        
        public override void Exit()
        {
            _player.InputSO.OnAttackPressed -= HandleAttackPressed;
            _animatorTrigger.OnAttackStatusChange -= HandleAttackStatusChange; 
            _animatorTrigger.OnAnimationComboEnd -= HandleAnimationComboEnd; 
            
            _aimController.rotationSpeed = _rotateSpeed;
            _movement.StopImmediately();
            _attackCompo.EndAttack();
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            
            Vector2 movementKey = _player.InputSO.MovementKey;
            _movement.SetMovementDirection(movementKey * _moveSpeed);

            if (_isCanNextAttack && _isPressed)
            {
                _attackCompo.Attack();
                _isPressed = false;
                _isCanNextAttack = false;
            }

            if (_isTriggerCall)
            {
                const string move = "MOVE";
                const string idle = "IDLE";
                _player.ChangeState(_player.InputSO.MovementKey == Vector2.zero ? idle : move);
            }
        }
        
        private void HandleAttackPressed() => _isPressed = true;
        
        private void HandleAnimationComboEnd() => _isCanNextAttack = true;

        private void HandleAttackStatusChange(bool isAttacking)
        {
            if (isAttacking == false)
                _isCanNextAttack = false;
            _moveSpeed = isAttacking ? 0.2f : 0.6f;
        }
    }
}