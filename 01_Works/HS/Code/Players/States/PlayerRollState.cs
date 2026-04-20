using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.States
{
    public class PlayerRollState : PlayerState
    {
        private PlayerAimController _aimController;
        private Vector3 _rollingDirection;
        
        public PlayerRollState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _aimController = entity.GetCompo<PlayerAimController>();
        }

        public override void Enter()
        {
            base.Enter();
            _aimController.isCanRotate = false;
            _movement.CanManualMovement = false;
            _rollingDirection = new Vector3(_player.transform.forward.x, 0, _player.transform.forward.z);
            
            _animatorTrigger.OnRollingStatusChange += HandleRollingStatusChange;
        }

        public override void Exit()
        {
            _aimController.isCanRotate = true;
            _movement.CanManualMovement = true;
            _movement.StartRollCollDown();
            _movement.SetAutoMovement(Vector3.zero);
            
            _animatorTrigger.OnRollingStatusChange -= HandleRollingStatusChange;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            const string move = "MOVE";
            const string idle = "IDLE";

            if (_isTriggerCall)
            {
                _player.ChangeState(_player.InputSO.MovementKey == Vector2.zero ? idle : move);
            }
        }
        
        private void HandleRollingStatusChange(bool isRolling)
        {
            _player.ChangePlayerLayer(isRolling);
            
            float velocity = isRolling ? 
                _movement.RollingSpeedStat.currentValue : _movement.RollingSpeedStat.currentValue * 0.4f;
            _movement.SetAutoMovement(_rollingDirection * velocity);
        }
    }
}