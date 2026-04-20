namespace _01_Works.HS.Code.Events
{
    public class UIEvents
    {
        public static readonly ChangeRollCollDown ChangeRollCollDown = new ChangeRollCollDown();
        public static readonly ChangeHealth ChangeHealth = new ChangeHealth();
    }

    public class ChangeRollCollDown : GameEvent
    {
        public float CoolDown;
        public float CurTimer;
        
        public ChangeRollCollDown Initializer(float curTimer, float coolDown)
        {
            CurTimer = curTimer;
            CoolDown = coolDown;
            return this;
        }
    }
    
    public class ChangeHealth : GameEvent
    {
        public int CurrentHealth;
        public int MaxHealth;
        
        public ChangeHealth Initializer(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            return this;
        }
    }
}