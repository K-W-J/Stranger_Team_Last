using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        protected AbilityCompo _abilityCompo;
        protected Entity _entity;
        public bool IsActiveAbility {get; private set;}
        
        public AbilityType abilityType;

        public virtual void InitializeAbility(AbilityCompo abilityCompo, Entity entity)
        {
            _abilityCompo = abilityCompo;
            _entity = entity;
        }
        
        public virtual void ActiveAbility(bool isActive) => IsActiveAbility = isActive;
    }
}