using UnityEngine;

namespace _01_Scripts.Sound
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "SO/Audio/Data", order = 0)]
    public class SoundSO : ScriptableObject
    {
        public AudioManager.SoundType soundType;
        public AudioClip clip;
        public string soundName;
    }
}