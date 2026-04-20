using System;
using System.Threading;
using UnityEngine;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY._01_Scripts.Dependencies;
using _01_Works.KHJ.Bullet;
using DG.Tweening;
using UnityEngine.Events;

namespace KWJ.MapObjects.Trap
{
    public class Turret : MonoBehaviour
    {
        public UnityEvent OnFireEvent;
        
        [SerializeField] private PoolItemSO _bullet;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _fireDelay;
        [SerializeField] private float _startDeley = 5f;
        
        [Space]
        
        [SerializeField] private Transform _gunBarrel;
        [SerializeField] private float _knockbackPower;
        [SerializeField] private float _knockbackSpeed;
        
        [SerializeField] private PoolManagerSO _poolManager;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private void Start()
        {
            StartFire();
        }

        public async void StartFire()
        {
            await Awaitable.WaitForSecondsAsync(_startDeley, _cts.Token);
            Fire();
        }

        [ContextMenu("Fire")]
        public async void Fire()
        {
            try
            {
                while (true)
                {
                    await Awaitable.WaitForSecondsAsync(_fireDelay, _cts.Token);
                    
                    OnFireEvent?.Invoke();

                    float localZ = _gunBarrel.localPosition.z;
                    
                    _gunBarrel.DOLocalMoveZ(localZ - _knockbackPower, _knockbackSpeed);
                    _gunBarrel.DOLocalMoveZ(localZ, _knockbackSpeed);
                    
                    CreateBullet();
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void OnDisable()
        {
            _cts.Cancel();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
        }

        private void CreateBullet()
        {
            Bullet bullet = _poolManager.Pop(_bullet).GameObject.GetComponent<Bullet>();
            bullet.transform.position = _muzzle.position;
            bullet.transform.rotation = _muzzle.rotation;
            bullet.Fire();
        }

        private void StopFire()
        {
            _cts.Cancel();
        }
    }
}