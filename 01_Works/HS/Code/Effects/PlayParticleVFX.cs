using UnityEngine;

namespace _01_Works.HS.Code.Effects
{
    public class PlayParticleVFX : MonoBehaviour, IPlayableVFX
    {
        [field:SerializeField] public string VFXName { get; private set; }
        [SerializeField] private bool isOnPosition;
        protected ParticleSystem []_particles;

        protected virtual void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        public virtual void PlayVFX(Vector3 position, Quaternion rotation)
        {
            if(isOnPosition == false)
                transform.SetPositionAndRotation(position, rotation);

            foreach(var particle in _particles)
                particle.Play(true); //트루는 안해줘도 되긴 해
        }

        public void StopVFX()
        {
            foreach(var particle in _particles)
                particle.Stop(true);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(VFXName) == false)
                gameObject.name = VFXName;
        }
    }
}