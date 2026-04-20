using System.Collections.Generic;
using System.Linq;
using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability
{
    public class AbilityCompo : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private Dictionary<AbilityType, Ability> _abilityDict;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _abilityDict = new Dictionary<AbilityType, Ability>();
            
            GetComponentsInChildren<Ability>().ToList()
                .ForEach(ability => _abilityDict.Add(ability.abilityType, ability));

            foreach (var ability in _abilityDict.Values)
            {
                ability.InitializeAbility(this, entity);
            }
        }
        
        public Ability GetAbility(AbilityType abilityType) => _abilityDict[abilityType];
        public void ActiveAbility(AbilityType abilityType, bool isActive) => _abilityDict[abilityType].ActiveAbility(isActive);
    }
}