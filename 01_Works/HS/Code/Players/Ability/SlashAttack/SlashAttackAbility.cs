using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability.SlashAttack
{
    public class SlashAttackAbility : Ability
    {
        [SerializeField] private GameEventChannelSO abilityChannel;
        [SerializeField] private PoolItemSO slashItem;
        [SerializeField] private PoolManagerMono poolManager;
        
        public override void InitializeAbility(AbilityCompo abilityCompo, Entity entity)
        {
            base.InitializeAbility(abilityCompo, entity);
            
            abilityChannel.AddListener<Events.SlashAttack>(HandleSlashAttack);
        }

        private void OnDestroy()
        {
            abilityChannel.RemoveListener<Events.SlashAttack>(HandleSlashAttack);
        }

        private void HandleSlashAttack(Events.SlashAttack evt)
        {
            if (IsActiveAbility == false) return;
            
            Slash slash = poolManager.Pop<Slash>(slashItem);
            slash.ThrowSlash(evt.Position, evt.Rotation, _entity.transform.forward, _entity);
        }
    }
}