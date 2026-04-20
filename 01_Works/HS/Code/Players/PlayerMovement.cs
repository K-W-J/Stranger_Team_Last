using System;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.Players.Stat;
using UnityEngine;
using UnityEngine.Events;

namespace _01_Works.HS.Code.Players
{
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float rollCollDown;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private StatDataSO moveSpeedStat;
        [field:SerializeField] public StatDataSO RollingSpeedStat {get; private set;}
        
        private Player _player;

        private Vector3 _movementDirection;
        private Vector3 _autoMovement;
        private Vector3 _velocity;
        private float _rollTimer;
        private float _verticalVelocity;
        private bool _isCanRoll = true;
        
        public bool CanManualMovement { get; set; } = true;
        public bool IsGround => controller.isGrounded;
        
        public UnityEvent<Vector3> OnMoveEvent;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        private void Start()
        {
            uiChannel.RaiseEvent(UIEvents.ChangeRollCollDown.Initializer(rollCollDown, rollCollDown));
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection = new Vector3(movementInput.x, 0, movementInput.y);
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = _movementDirection;
                _velocity *= moveSpeedStat.currentValue * Time.fixedDeltaTime;
            }
            else
            {
                _velocity = _autoMovement * Time.fixedDeltaTime;
            }
        }

        private void Move()
        {
            if (_player.IsDead) return;

            // _velocity.y = 0;
            controller.Move(_velocity);
            OnMoveEvent?.Invoke(_velocity.normalized);
        }
        
        private void ApplyGravity()
        {
            if (_player.InputSO.isEnable == false) return;
            
            if(IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f;
            else 
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            
            _velocity.y = _verticalVelocity;
            // _player.transform.position = new Vector3(
            //     _player.transform.position.x, 0, _player.transform.position.z);
            // _player.transform.rotation = Quaternion.Euler(
            //     0, _player.transform.rotation.eulerAngles.y, 0);
        }

        #region Rolling
        public bool CheckCanRoll()
        {
            if (_isCanRoll)
            {
                _rollTimer = 0;
                _isCanRoll = false;
                uiChannel.RaiseEvent(UIEvents.ChangeRollCollDown.Initializer(0, rollCollDown));
                return true;
            }
            return false;
        }

        public async void StartRollCollDown()
        {
            try
            {
                while (_rollTimer <= rollCollDown)
                {
                    await Awaitable.EndOfFrameAsync();
                    _rollTimer += Time.deltaTime;
                    uiChannel.RaiseEvent(UIEvents.ChangeRollCollDown.Initializer(_rollTimer, rollCollDown));
                }

                _isCanRoll = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        #endregion
        
        public void SetAutoMovement(Vector3 autoMovement) => _autoMovement = autoMovement;
        public void StopImmediately() => _movementDirection = Vector3.zero;
    }
}