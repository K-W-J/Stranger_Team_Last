using System.Collections.Generic;
using System.Linq;
using _01_Works.HS.Code.Feedbacks;
using UnityEngine;

namespace _01_Works.JY._01_Scripts.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private GameObject root;
        [SerializeField] private float blinkDuration = 0.15f;
        [SerializeField] private float blinkIntensity = 0.2f;

        private List<MeshRenderer> meshList = new List<MeshRenderer>();
        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");
        
        private void Start()
        {
            meshList = root.GetComponentsInChildren<MeshRenderer>().ToList();
        }
        
        public override async void CreateFeedback()
        {
            try
            {
                foreach (var mesh in meshList)
                    mesh.material.SetFloat(_blinkHash, blinkIntensity);
                meshRenderer.material.SetFloat(_blinkHash, blinkIntensity);
                await Awaitable.WaitForSecondsAsync(blinkDuration);
                StopFeedback();
            }
            catch (System.Exception)
            {
                // This can happen when the object is destroyed during the await.
            }
        }

        public override void StopFeedback()
        {
            if (meshList != null)
            {
                foreach (var mesh in meshList)
                    mesh.material.SetFloat(_blinkHash, 0f);
            }
        
            if(meshRenderer != null)
                meshRenderer.material.SetFloat(_blinkHash, 0f);
        }
    }
}