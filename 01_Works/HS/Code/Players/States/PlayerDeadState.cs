using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.States
{
    public class PlayerDeadState : PlayerState
    {
        private float _timer;
        
        public PlayerDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _timer = 0;
        }

        public override void Update()
        {
            base.Update();

            _timer += Time.deltaTime;

            if (_timer >= 1)
            {
                Time.timeScale = 0.65f;
            }
            else
            {
                Time.timeScale = _player.deadTimeCurve.Evaluate(_timer);
            }
        }
    }
}