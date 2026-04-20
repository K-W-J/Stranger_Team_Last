using KWJ.OverlapChecker;
using UnityEngine;

namespace KWJ.MapObjects.Trap
{
    public class Landmine : Explosive
    {
        [SerializeField] private BoxOverlapChecker _boxChecker;
        [SerializeField] private GameObject _light;

        private float _currentLightTime;

        private void Start()
        {
            _light.SetActive(false);
        }

        private void Update()
        {
            if (m_isExplosion)
            {
                LightHideShow();
                return;
            }
            
            if (_boxChecker.BoxOverlapCheck())
            {
                ExplosionDelay();
            }
        }

        private void LightHideShow()
        {
            if (m_explosionDelay / 5f <= _currentLightTime)
            {
                bool isActive = !_light.activeSelf;
                _light.SetActive(isActive);
                _currentLightTime = 0f;
                return;
            }

            _currentLightTime += Time.deltaTime;
        }
    }
}