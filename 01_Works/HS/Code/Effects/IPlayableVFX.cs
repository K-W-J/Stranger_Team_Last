using UnityEngine;

namespace _01_Works.HS.Code.Effects
{
    public interface IPlayableVFX
    {
        public string VFXName { get; }
        public void PlayVFX(Vector3 position,Quaternion rotation);
        public void StopVFX();
    }
}