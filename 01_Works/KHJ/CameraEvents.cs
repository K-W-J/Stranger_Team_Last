
using _01_Works.HS.Code.Events;

namespace _01_Works.KHJ
{
    public static class CameraEvents
    {
        public static ImpulseEvent ImpulseEvent = new ImpulseEvent();
    }

    public class ImpulseEvent : GameEvent
    {
        public float power;

        public ImpulseEvent Initializer(float power)
        {
            this.power = power;
            return this;
        }
    }
}