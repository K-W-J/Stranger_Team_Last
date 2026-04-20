using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Entities.FSM;

namespace _01_Works.HS.Code.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;
        protected PlayerMovement _movement;
        
        protected PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<PlayerMovement>();
            
        }
    }
}