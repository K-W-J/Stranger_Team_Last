using System;
using _01_Works.HS.Code.Combat;
using UnityEngine;
using KWJ.OverlapChecker;
using UnityEngine.Events;

namespace KWJ.MapObjects.Trap
{
    public class SplinterTrap : MonoBehaviour
    {
        public UnityEvent OnTrapEvent;
        
        [SerializeField] private TrapAnimator _trapAnimator;
        [SerializeField] private BoxOverlapChecker _boxChecker;
        [SerializeField] private int _damage;
        
        [SerializeField] private float _startDelay;

        private void Start()
        {
            _trapAnimator.SetAnimationSpeed(0f);
            
            StartAnimation();
        }

        private async void StartAnimation()
        {
            await Awaitable.WaitForSecondsAsync(_startDelay);
            
            _trapAnimator.SetAnimationSpeed(1f);
        }

        public void TrapTakeDamage()
        {
            OnTrapEvent?.Invoke();
            
            GameObject[] targets = _boxChecker.GetOverlapData();

            foreach (var target in targets)
            {
                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyDamage(_damage, null);
                }
            }
        }
    }
}