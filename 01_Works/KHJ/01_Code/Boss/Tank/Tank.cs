using _01_Works.HS.Code.Combat;
using _01_Works.JY.Enemies;
using _01_Works.KHJ.Boss;
using KWJ.Etc;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace _01_Works.KHJ.Boss
{
    public class Tank : Boss
    {
        [SerializeField] private LayerMask _targetMask;

        // [SerializeField] private NavMovement _movement;
        [SerializeField] public Transform GunBarrel;



        [SerializeField] public Transform[] muzzles;

        public PlayableDirector StartTimeline;
        public PlayableDirector DeathTimeline;
        public TimelineAsset timeline;

        private Transform lastPoint;   // ���� ���õ� ����Ʈ
        private Transform currentPoint;

        private Transform _movePoint;

        protected override void Start()
        {
            base.Start();
            Play();
        }

        public void Play()
        {
            BossScene.Instance.BossStart();
        }

        public void Death()
        {
            BossScene.Instance.BossDeath();
        }

        public void HIt(int a, int b)
        {
            HpBar.Instance.ChangeHpBar(a, b);
        }

        private void OnTriggerStay(Collider other)
        {
            if (((1 << other.gameObject.layer) & _targetMask) != 0)
            {
                Debug.Log(1);
                if (other.transform.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(2);
                    damageable.ApplyDamage(1, this);
                }
            }
        }

        private void Update()
        {
            if (_movePoint == null || Vector3.Distance(_movePoint.position, transform.position) < 9)
            {
                ChooseRandomMovePoint();
            }
        }


        public void ChooseRandomMovePoint()
        {
            _movement.SetStop(false);

            Transform point = BossPoint.Instance.MovePoints[UnityEngine.Random.Range(0, BossPoint.Instance.MovePoints.Length)];
            while (point == _movePoint)
            {
                point = BossPoint.Instance.MovePoints[UnityEngine.Random.Range(0, BossPoint.Instance.MovePoints.Length)];
            }
            _movePoint = point;
            _movement.SetDestination(point.position);
        }
    }
}
