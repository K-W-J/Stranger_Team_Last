using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _01_Works.HS.Code.Feedbacks
{
    public class VignetteFeedback : Feedback
    {
        [SerializeField] private Color intensityColor;
        [SerializeField] private Volume volume;
        [SerializeField] private float intensity;
        [SerializeField] private float duration = 0.3f;
        
        private Vignette _vignette;
        
        public override void CreateFeedback()
        {
            if (volume == null || volume.profile == null) return;
            
            if (volume.profile.TryGet(out _vignette))
            {
                _vignette.color.value = intensityColor;
                DOTween.To(()=> 0f, x => _vignette.intensity.value = x,
                    intensity, duration).SetLoops(2, LoopType.Yoyo);
            }
        }

        public override void StopFeedback()
        {
            
        }
    }
}