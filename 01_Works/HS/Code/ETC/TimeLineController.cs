using Input;
using UnityEngine;
using UnityEngine.Playables;

namespace _01_Works.HS.Code.ETC
{
    public class TimeLineController : MonoBehaviour
    {
        [SerializeField] private bool isShowTimeLine = true;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private TimeLineDataSO timeLineData;
        
        private PlayableDirector _director;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
        }

        private async void Start()
        {
            Time.timeScale = 1;
            playerInput.isEnable = false;
            
            await Awaitable.WaitForSecondsAsync(3.5f);
            
            if (isShowTimeLine)
            {
                _director.Play();
                await Awaitable.WaitForSecondsAsync((float)_director.duration);
                await Awaitable.WaitForSecondsAsync(0.3f);
            }
            
            playerInput.isEnable = true;
        }
    }
}