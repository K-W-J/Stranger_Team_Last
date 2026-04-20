using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.States
{
    public class PlayerMoveState : PlayerState, ICanAttackState, ICanRollState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            const string idle = "IDLE";
            Vector2 movementKey = _player.InputSO.MovementKey;
            
            _movement.SetMovementDirection(movementKey);
            if(movementKey.magnitude < _inputThreshold)
                _player.ChangeState(idle);
        }
    }
}