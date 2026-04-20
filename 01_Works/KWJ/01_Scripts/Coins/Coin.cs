using System.Collections;
using _01_Works.HS.Code.ObjectPool.RunTime;
using UnityEngine;

namespace KWJ.Coins
{
    public class Coin : MonoBehaviour, IPoolable
    {
        [field : SerializeField] public PoolItemSO PoolItem { get; set; }
        [SerializeField] private float poolDelay;
        
        [SerializeField] private float _rotationSpeed;
        
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _height = 0.5f;
        public GameObject GameObject => gameObject;
        
        private Pool _pool;

        private bool _isGetCoin;
        
        private void Update()
        {
            transform.localEulerAngles += Vector3.up * (_rotationSpeed * Time.deltaTime);
        }

        public void DropCoin(Vector3 position)
        {
            transform.position = position;
            
            Vector3 end = position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            
            StartCoroutine(ParabolaMotion(position, end));
            DelayedPooling();
        }
        
        public void AcquireCoin(Transform targetTrm)
        {
            if(_isGetCoin) return;

            _isGetCoin = true;
            StartCoroutine(MoveCoin(targetTrm));
        }

        private async void DelayedPooling()
        {
            await Awaitable.WaitForSecondsAsync(poolDelay);
            _pool.Push(this);
        }
        
        private IEnumerator ParabolaMotion(Vector3 start, Vector3 end)
        {
            float time = 0f;

            while (time < _duration)
            {
                float t = time / _duration;

                // x,z는 직선 보간
                Vector3 pos = Vector3.Lerp(start, end, t);

                // y는 포물선 곡선 (최고 높이까지 올랐다가 내려오기)
                pos.y = Mathf.Lerp(start.y, end.y, t) 
                        + _height * (1 - (2 * t - 1) * (2 * t - 1)); 

                transform.position = pos;

                time += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
        }
        private IEnumerator MoveCoin(Transform targetTrm)
        {
            while (true)
            {
                yield return null;
                
                Vector3 dir = transform.position - targetTrm.position;
                
                if (dir.sqrMagnitude <= 1f)
                    break;

                transform.position = Vector3.Lerp(transform.position, targetTrm.position, 4f * Time.deltaTime);
            }
            
            CoinManager.Instance.AddRandomCoin();
            AudioManager.Instance.PlaySfx("EATCOIN");
            _pool.Push(this);
        }
        
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            _isGetCoin = false;
        }
    }
}