using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Works.HS.Code.UI
{
    public class FadeUI : MonoBehaviour
    {
        private Image _fadeImage;
        
        private void Awake()
        {
            _fadeImage = GetComponent<Image>();
        }

        private async void Start()
        {
            await Awaitable.WaitForSecondsAsync(3.5f);
            
            Fade(false, (() => Debug.Log("sdafasd")));
        }

        public void Fade(bool isFade, Action callback)
        {
            if (isFade)
                _fadeImage.DOFade(1, 0.5f).OnComplete(callback.Invoke);
            else
                _fadeImage.DOFade(0, 0.5f).OnComplete(callback.Invoke);
        }
    }
}