using UnityEngine;

namespace _01_Works.HS.Code.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void StopFeedback();
    }
}