using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Players.Ability;
using _01_Works.HS.Code.Players.Stat;
using UnityEngine;

namespace _01_Works.HS.Code.Players
{
    public class PlayerItemCompo : MonoBehaviour, IEntityComponent
    {
        public Player Player { get; private set; }
        
        public AbilityCompo AbilityCompo { get; private set; }
        public StatCompo StatCompo { get; private set; }
        
        public void Initialize(Entity entity)
        {
            Player = entity as Player;
            
            AbilityCompo = entity.GetCompo<AbilityCompo>();
            StatCompo = entity.GetCompo<StatCompo>();
        }
    }
}