using Input;
using Unity.Cinemachine;
using UnityEngine;

namespace _01_Works.HS.Code.ETC
{
    public class CameraOffsetController : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputSO;
        
        private CinemachineRotationComposer _rotationComposer;

        private void Awake()
        {
            _rotationComposer = GetComponent<CinemachineRotationComposer>(); 
        }

        private void LateUpdate()
        {
            Vector3 worldPosition = new Vector3(
                -Mathf.Clamp(inputSO.ScreenPosition.x / Screen.width - 0.5f, -0.5f, 0.5f),
                 Mathf.Clamp(inputSO.ScreenPosition.y / Screen.width - 0.3f, -0.45f, 0.45f), 0);
            
            const float posMultiplier = 0.2f;
            _rotationComposer.Composition.ScreenPosition = worldPosition * posMultiplier;
        }
    }
}