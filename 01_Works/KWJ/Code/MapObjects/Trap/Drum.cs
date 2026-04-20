
using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;

namespace KWJ.MapObjects.Trap
{
    public class Drum : Explosive, IDamageable
    {
        public void ApplyDamage(int damage, Entity dealer)
        {
            if(m_isExplosion) return;
            
            StartCoroutine(Explosion());
        }
    }
}