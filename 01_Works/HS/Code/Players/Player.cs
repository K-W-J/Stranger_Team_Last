using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Entities.FSM;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.Players.Stat;
using _01_Works.HS.Code.Players.States;
using _01_Works.HS.Code.UI;
using Blade.FSM;
using Input;
using KWJ.Coins;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01_Works.HS.Code.Players
{
    public class Player : Entity, IDamageable
    {
        [field:SerializeField] public PlayerInputSO InputSO { get; private set; }
        
        [SerializeField] private StateDataSO[] states;
        [SerializeField] private string playerLayer;
        [SerializeField] private string ghostLayer;
        [SerializeField] private StatDataSO healthStat;
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private LayerMask whatIsCoin;
        [SerializeField] private FadeUI fadeUI;
        
        private EntityStateMachine _stateMachine;
        private Material _material;
        private PlayerMovement _movement;
        public Health HealthCompo { get; private set; }
        private CharacterController _controller;
        private EntityVFX _entityVFX;

        // 콜라이더를 꺼도 충돌 체크가 달라서 bool값으로도 한 번 더 체크
        public bool IsCanHit { get; private set; }
        public AnimationCurve deadTimeCurve;

        private readonly int _blinkValueHash = Shader.PropertyToID("_BlinkValue");
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            _movement = GetCompo<PlayerMovement>();
            HealthCompo = GetCompo<Health>();
            _entityVFX = GetCompo<EntityVFX>();
            _material = GetComponentInChildren<Renderer>().sharedMaterial;
    
            InputSO.OnRollPressed += HandleRollKeyPressed;
            healthStat.OnChangeValue += HandleChangeHealthStat;
        }

        private void OnDestroy()
        {
            InputSO.OnRollPressed -= HandleRollKeyPressed;
            healthStat.OnChangeValue -= HandleChangeHealthStat;
            
            // 걍 게임 꺼져도 유지되길래 초기화 해줌
            _material.SetFloat(_blinkValueHash, 0f); 
        }

        protected override void Start()
        {
            _stateMachine.ChangeState("IDLE");
            _material.SetFloat(_blinkValueHash, 0);
            IsCanHit = true;
            HealthCompo.SetUpHealth((int)healthStat.defaultValue);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer("Bullet"), false);
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();

            CheckCoin();
        }

        private void CheckCoin()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, whatIsCoin);

            if (colliders.Length < 1) return;
                
            foreach (var col in colliders)
                col.GetComponent<Coin>().AcquireCoin(transform);
        }

        public async void ApplyDamage(int damage, Entity dealer)
        {
            if (IsCanHit == false) return;
            if (IsDead) return;
            
            HealthCompo.ApplyDamage(damage);

            if (IsDead) return;
            
            IsCanHit = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer("Bullet"), true);
            
            _material.SetFloat(_blinkValueHash, 1);
            await Awaitable.WaitForSecondsAsync(0.1f);
            for (int i = 0; i < 3; i++)
            {
                await Awaitable.WaitForSecondsAsync(0.2f);
                _material.SetFloat(_blinkValueHash, 0f);
                await Awaitable.WaitForSecondsAsync(0.2f);
                _material.SetFloat(_blinkValueHash, 0.13f);
            }
            await Awaitable.WaitForSecondsAsync(0.2f);
            _material.SetFloat(_blinkValueHash, 0f);
            await Awaitable.WaitForSecondsAsync(0.1f);
            
            IsCanHit = true;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer("Bullet"), false);
        }

        public void HandleRollKeyPressed()
        {
            const string roll = "ROLL";
            
            if (GetCurrentState() is ICanRollState && _movement.CheckCanRoll())
                _stateMachine.ChangeState(roll);   
        }
        
        public void ChangeCanHit(bool isCanHit)
        {
            this.IsCanHit = isCanHit;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer("Bullet"), !isCanHit);
        }
        
        [ContextMenu("Heal")]
        public void Heal()
        {
            HealthCompo.Heal();
            _entityVFX.PlayVFX("HolyExplosion", transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfx("HEAL");
        }
        
        public void HandleAnimationMove(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            if (IsDead)
            {
                transform.position += deltaPosition;
            } 
        }

        public async void HandleEndEvent()
        {
            await Awaitable.WaitForSecondsAsync(6.5f);
            fadeUI.Fade(true, () => SceneManager.LoadScene(0));
        }
        private void HandleChangeHealthStat(float maxHealth)
            => HealthCompo.EditMaxHealth((int)maxHealth);
        public void ChangePlayerLayer(bool isGhost) // 플레이어 충돌 레이어 변경
            => gameObject.layer = LayerMask.NameToLayer(isGhost ? ghostLayer : playerLayer);
        public void HandleChangeHealth(int currentHealth, int maxHealth) // 체력 변경 이벤트 핸들
            => uiEventChannel.RaiseEvent(UIEvents.ChangeHealth.Initializer(currentHealth, maxHealth));
        public void ChangeState(string stateName) // State 변경
            => _stateMachine.ChangeState(stateName);
        public PlayerState GetCurrentState() // 현재 State 가져오기
            => _stateMachine.GetCurrentState() as PlayerState;
    }
}