using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.KHJ.Skill
{
    public class Skill : MonoBehaviour
    {
        public delegate void CooldownInfo(float current, float duration);

        [SerializeField] protected float cooldownDuration = 1.0f;

        protected float _cooldownTimer;
        protected Entity _owner;
        protected SkillComponent _skillComponent;

        public bool IsCooldown => _cooldownTimer > 0.0f;
        public event CooldownInfo OnCooldownInfo;

        public virtual void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            _owner = owner;
            _skillComponent = skillComponent;
        }

        protected virtual void Update()
        {
            if (_cooldownTimer <= 0) return;

            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0)
            {
                _cooldownTimer = 0;
            }
            OnCooldownInfo?.Invoke(_cooldownTimer, cooldownDuration);
        }

        public virtual void UseSkill()
        {
            _cooldownTimer = cooldownDuration;
        }

       
    }
}

