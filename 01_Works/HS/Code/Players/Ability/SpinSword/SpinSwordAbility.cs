using System.Linq;
using _01_Works.HS.Code.Effects;
using _01_Works.JY.Enemies;
using UnityEngine;

namespace _01_Works.HS.Code.Players.Ability.SpinSword
{
    public class SpinSwordAbility : Ability
    {
        [SerializeField] private SpinSword spinSwordPrefab;
        [SerializeField] private float detectDistance = 5f;
        [SerializeField] private LayerMask whatIsEnemy;

        public override void ActiveAbility(bool isActive)
        {
            base.ActiveAbility(isActive);

            if (isActive == false) return;
            
            SpinSword spinSword = Instantiate(spinSwordPrefab, _entity.transform.position + Vector3.up * 1.25f, Quaternion.identity);
            spinSword.SetupSword(_entity, this);
        }

        public bool TryGetClosestEnemyTrm(ref Transform trm)
        {
            var colliders = Physics.OverlapSphere(_entity.transform.position,
                detectDistance, whatIsEnemy);
            if (colliders.Length > 0)
            {
                trm = colliders.OrderBy(col => Vector3.Distance(_entity.transform.position, col.transform.position))
                    .First().transform;
                return true;
            }

            return false;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectDistance);
            Gizmos.color = Color.white;
        }
        #endif
    }
}