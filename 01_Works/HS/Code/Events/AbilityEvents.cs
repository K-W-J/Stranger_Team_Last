using UnityEngine;

namespace _01_Works.HS.Code.Events
{
    public class AbilityEvents
    {
        public static readonly SlashAttack SlashAttack = new SlashAttack();
    }
    
    public class SlashAttack : GameEvent
    {
        public Quaternion Rotation;
        public Vector3 Position;
        
        public SlashAttack Initializer(Quaternion rotation, Vector3 position)
        {
            Rotation = rotation;
            Position = position;
            return this;
        }
    }
}