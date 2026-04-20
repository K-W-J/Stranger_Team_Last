using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players
{
    public class UpdateMoveAnimator : MonoBehaviour, IEntityComponent
    {
        private Animator _animator;
        
        private readonly int _xMoveHash = Animator.StringToHash("MOVE_X");
        private readonly int _zMoveHash = Animator.StringToHash("MOVE_Z");

        private Player _player;
        private Vector3 _moveDirection;

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _animator = GetComponent<Animator>();
        }
        
        public void HandleMoveAnimation(Vector3 direction)
        {
            direction = _player.transform.InverseTransformDirection(direction);
            _moveDirection.x = Mathf.Lerp(_moveDirection.x, direction.x, Time.deltaTime * 7.5f);
            _moveDirection.z = Mathf.Lerp(_moveDirection.z, direction.z, Time.deltaTime * 7.5f);
            
            _animator.SetFloat(_xMoveHash, _moveDirection.x);
            _animator.SetFloat(_zMoveHash, _moveDirection.z);
        }
    }
}