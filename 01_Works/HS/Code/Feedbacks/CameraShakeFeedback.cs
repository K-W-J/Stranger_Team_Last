using _01_Works.HS.Code.Events;
using _01_Works.KHJ;
using UnityEngine;

namespace _01_Works.HS.Code.Feedbacks
{
    public class CameraShakeFeedback : Feedback
    {
        [SerializeField] private float impulsePower;
        [SerializeField] private GameEventChannelSO cameraChannel;
        
        public override void CreateFeedback()
        {
            ImpulseEvent evt = CameraEvents.ImpulseEvent.Initializer(1.5f);
            cameraChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
            
        }
    }
}