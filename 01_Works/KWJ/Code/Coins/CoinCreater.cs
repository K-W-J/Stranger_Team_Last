using System.Collections;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY._01_Scripts.Dependencies;
using UnityEngine;

namespace KWJ.Coins
{
    public class CoinCreater : MonoBehaviour
    {
        [field : SerializeField] public PoolItemSO PoolItem { get; set; }
        
        [SerializeField] private PoolManagerSO _poolManager;

        [ContextMenu("CreateCoin")]
        public void CreateCoin()
        {
            for (int i = 0; i < CoinManager.Instance.RandomCoinCount(); i++)
            {
                Coin coin = _poolManager.Pop(PoolItem).GameObject.GetComponent<Coin>();
                coin.DropCoin(transform.position);
            }
        }
    }
}