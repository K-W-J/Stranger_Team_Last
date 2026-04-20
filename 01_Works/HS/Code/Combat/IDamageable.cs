using _01_Works.HS.Code.Entities;

namespace _01_Works.HS.Code.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(int damage, Entity dealer);
    }
}