using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY._01_Scripts.Bullet;
using _01_Works.JY.Managers;
using System.Collections;
using UnityEngine;

namespace _01_Works.KHJ.Bullet
{
    public class SplittingBullet : Bullet  // 이름 변경
    {
        private float _moveTime = 2;
        private int splitCount = 19;

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        public override void Fire()
        {
            base.Fire();
            StartCoroutine(WaitTime());
        }

        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(_moveTime);
            Split();
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        void Split()
        {
            float angleStep = 360f / splitCount;
            for (int i = 0; i < splitCount; i++)
            {
                float angle = i * angleStep;
                Bullet bullet = BulletManager.Instance.CreateBullet(BulletType.BULLET);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                bullet.Fire();
            }
            _pool.Push(this);
        }
    }
}
