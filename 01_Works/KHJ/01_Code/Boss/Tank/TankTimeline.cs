using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace _01_Works.KHJ.Boss
{
    public class TankTimeline : MonoBehaviour
    {
        public PlayableDirector StartTimeline;
        public PlayableDirector DeathTimeline;
        public TimelineAsset timeline;

        private void Start()
        {
            Play();
        }

        // 시작 타임라인 재생
        public void Play()
        {
            if (StartTimeline != null)
                StartTimeline.Play();
        }

        // 죽음 타임라인 재생
        public void Death()
        {
            if (DeathTimeline != null)
                DeathTimeline.Play();
        }
    }
}
