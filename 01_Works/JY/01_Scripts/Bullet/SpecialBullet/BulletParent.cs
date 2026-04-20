using System;
using System.Collections;
using System.Collections.Generic;
using _01_Works.HS.Code.Events;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY.Events;
using UnityEngine;

namespace _01_Works.JY._01_Scripts.Bullet.SpecialBullet
{
    public class BulletParent : MonoBehaviour
    {
        [Header("Formation")]
        [SerializeField] private float radius = 2f;
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float growthDuration = 1f;

        [Header("Spawning")]
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private PoolItemSO reflectBulletPool;
        [SerializeField] private int bulletCount;
        [SerializeField] private LayerMask whatIsPlayer;

        private Pool _pool;
        public readonly List<KHJ.Bullet.Bullet> bullets = new List<KHJ.Bullet.Bullet>();
        private float _currentRadius;

        private void OnEnable()
        {
            transform.rotation = Quaternion.identity;
            _currentRadius = 0f;
            StopAllCoroutines();
            bullets.Clear();
            SpawnAndArrangeBullets();
        }
        
        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            if (_currentRadius < radius)
            {
                _currentRadius += (radius / growthDuration) * Time.deltaTime;
                _currentRadius = Mathf.Min(_currentRadius, radius);
            }
            UpdateBulletPositions();
        }

        private void SpawnAndArrangeBullets()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                var bulletEvt = BulletEvents.SpawnBulletsInCircle.Initializer
                    (transform.position, transform.eulerAngles, reflectBulletPool, transform, 0f, true);
                eventChannel.RaiseEvent(bulletEvt);
            }

            StartCoroutine(CollectBulletsAfterDelay());
        }

        private IEnumerator CollectBulletsAfterDelay()
        {
            yield return new WaitForEndOfFrame();

            foreach (var bullet in transform.GetComponentsInChildren<KHJ.Bullet.Bullet>())
            {
                bullets.Add(bullet);
                if (bullet.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.isKinematic = true;
                }
            }
            UpdateBulletPositions();
        }

        public bool CheckListInBullet()
        {
            if (transform.childCount <= 0) return true;
            
            foreach (var bullet in bullets)
            {
                if (bullet.gameObject.activeSelf)
                    return false;
            }
            return true;
        }

        private void UpdateBulletPositions()
        {
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i] != null)
                {
                    float angle = i * angleStep * Mathf.Deg2Rad;

                    float x = _currentRadius * Mathf.Cos(angle);
                    float z = _currentRadius * Mathf.Sin(angle);

                    bullets[i].transform.localPosition = new Vector3(x, 0, z);
                }
            }
        }
    }
}