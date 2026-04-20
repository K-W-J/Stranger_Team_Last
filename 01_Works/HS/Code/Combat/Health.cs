using _01_Works.HS.Code.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace _01_Works.HS.Code.Combat
{
    public class Health : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private int _maxHealth;

        public int CurrentHealth { get; private set; }
        
        // 첫 번째 인수 : 현재 체력, 두 번째 인수 최대 체력
        public UnityEvent<int, int> OnSetHealthEvent;
        public UnityEvent<int, int> OnDamageEvent;
        public UnityEvent<int, int> OnHealEvent;
        public UnityEvent<int, int> OnDeadEvent;
        
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;   
        }

        public virtual void SetUpHealth(int health) // 시작할떄 쓰는거
        {
            _maxHealth = health;
            CurrentHealth = health;
            OnSetHealthEvent?.Invoke(CurrentHealth, _maxHealth);
        }

        public void EditMaxHealth(int health)
        {
            int offset = health - _maxHealth;
            _maxHealth += offset;
            CurrentHealth += offset;
            OnSetHealthEvent?.Invoke(CurrentHealth, _maxHealth);
        }

        public virtual void ApplyDamage(int damage)
        {
            if (_entity.IsDead) return;
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
            OnDamageEvent?.Invoke(CurrentHealth, _maxHealth);

            Debug.Log(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                Debug.Log("죽음 보낸다");
                OnDeadEvent?.Invoke(CurrentHealth, _maxHealth);
                _entity.IsDead = true;
            }
        }

        public void Heal()
        {
            if (_entity.IsDead) return;
            
            CurrentHealth = Mathf.Clamp(CurrentHealth + 1, 0, _maxHealth);
            OnHealEvent?.Invoke(CurrentHealth, _maxHealth);
        }

        [ContextMenu("TestDamage")]
        public void TestDamage()
        {
            ApplyDamage(1);
        }
    }
}