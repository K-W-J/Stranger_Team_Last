using _01_Works.HS.Code.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blade.SkillSystem
{
    public class RoundDecal : MonoBehaviour, IPoolable
    {
        public DecalProjector decalProjector;
        [SerializeField] private float depth = 3f;
        public Pool _pool;
        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;

        public void SetProjectorActive(bool isActive)
        {
            decalProjector.enabled = isActive;
        }

        public void SetDecalSize(float radius)
        {
            decalProjector.size = new Vector3(2 * radius, 2 * radius, depth);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
        }

        public void Pade()
        {
            StartCoroutine(PadeCoroutine());
        }

        private IEnumerator PadeCoroutine()
        {
            DOTween.To(() => decalProjector.fadeFactor, x => decalProjector.fadeFactor = x, 0f, 0.5f);

            yield return new WaitForSeconds(1f);
            decalProjector.fadeFactor = 1;
            _pool.Push(this);
        }

    }
}