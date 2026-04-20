using _01_Works.HS.Code.Entities;

namespace _01_Works.HS.Code.Players.States
{
    public class PlayerIdleState : PlayerState, ICanAttackState, ICanRollState
    {
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        
        public override void Update()
        {
            base.Update();
            const string move = "MOVE";
            var movementKey = _player.InputSO.MovementKey;
            
            if (_player.InputSO.isEnable == false) return;
            
            _movement.SetMovementDirection(movementKey);
            if(movementKey.magnitude > _inputThreshold)
                _player.ChangeState(move);
        }
    }
}