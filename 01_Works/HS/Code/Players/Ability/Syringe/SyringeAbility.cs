using System;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Players.Stat;

namespace _01_Works.HS.Code.Players.Ability.Syringe
{
    public class SyringeAbility : Ability
    {
        private StatCompo _statCompo;

        public override void InitializeAbility(AbilityCompo abilityCompo, Entity entity)
        {
            base.InitializeAbility(abilityCompo, entity);
            _statCompo = entity.GetCompo<StatCompo>();
        }

        public override void ActiveAbility(bool isActive)
        {
            base.ActiveAbility(isActive);

            if (isActive)
            {
                _statCompo.AddStatValue(StatType.Health, 4);
            }
            else
            {
                _statCompo.RemoveStatValue(StatType.Health, 4);
            }
        }
    }
}