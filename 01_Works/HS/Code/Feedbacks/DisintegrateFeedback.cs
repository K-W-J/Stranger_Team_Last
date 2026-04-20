using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace _01_Works.HS.Code.Feedbacks
{
    public class DisintegrateFeedback : Feedback
    {
        [SerializeField] private float delayTime = 3f;
        [SerializeField] private VisualEffect feedbackEffect;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private float endDissolveValue = 0;
        private bool _isAlreadyStart = false;

        private readonly int _dissolveHeight = Shader.PropertyToID("_DissolveHeight");
        private readonly int _isDissolve = Shader.PropertyToID("_IsDissolve");

        public UnityEvent FeedbackComplete;
        
        public override void CreateFeedback()
        {
            if (_isAlreadyStart) return;

            _isAlreadyStart = true;
            meshRenderer.material.SetInt(_isDissolve, 1);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delayTime);
            seq.AppendCallback(() => feedbackEffect.Play());
            seq.Append(meshRenderer.material.DOFloat(endDissolveValue, _dissolveHeight, 1.2f));
            seq.AppendInterval(2f); //2초 대기
            seq.OnComplete(() => FeedbackComplete?.Invoke());
        }

        public override void StopFeedback()
        {
            
        }
    }
}