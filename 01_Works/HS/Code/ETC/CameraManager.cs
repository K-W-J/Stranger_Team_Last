using _01_Works.HS.Code.Events;
using _01_Works.KHJ;
using Unity.Cinemachine;
using UnityEngine;

namespace _01_Works.HS.Code.ETC
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            cameraChannel.AddListener<ImpulseEvent>(HandleCameraImpulse);
        }
        
        private void OnDestroy()
        {
            cameraChannel.RemoveListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void HandleCameraImpulse(ImpulseEvent evt)
        {
            impulseSource.GenerateImpulse(evt.power);
        }
    }
}