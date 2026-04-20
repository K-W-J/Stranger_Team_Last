using System;
using UnityEngine;
using UnityEngine.Events;

namespace KWJ.MapObjects.Trap
{
    public class TrapAnimator : MonoBehaviour
    {
        public UnityEvent OnAnimationSignalEvent;
        public UnityEvent OnAnimationEndEvent;
        
        [SerializeField] private Animator _animator;

        public void OnAnimationSignal() => OnAnimationSignalEvent?.Invoke();
        public void OnAnimationEnd() => OnAnimationEndEvent?.Invoke();
        public void SetAnimationSpeed(float AnimaSpeed)
        {
            _animator.speed = AnimaSpeed;
        }

        private void StopAnimation()
        {
            _animator.StopPlayback();
        }
    }
}