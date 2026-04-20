using _01_Works.HS.Code.Players.Ability;
using TMPro;
using UnityEngine;

namespace KWJ.Coins
{
    public class CoinManager : MonoSingleton<CoinManager>
    {
        [SerializeField] private int _randCoinCountMin;
        [SerializeField] private int _randCoinCountMax;
        [SerializeField] private int _randCoinMin;
        [SerializeField] private int _randCoinMax;
        [SerializeField] private IncreaseMoneyAbility increaseMoneyAbility;
        
        [Space]
        
        [SerializeField] private TextMeshProUGUI _coinText;
        
        public int CurrentCoin => _currentCoin;
        
        [Space]
        [SerializeField] private int _currentCoin;

        public bool HasEnoughCoins(int required)
        {
            if (_currentCoin - required < 0)
                return false;
            
            DecreaseCoin(required);
            return true;
        }

        public int RandomCoinCount()
        {
            return Random.Range(_randCoinCountMin, _randCoinCountMax);
        }
        
        public void AddRandomCoin()
        {
            int randCoin = Random.Range(_randCoinMin, _randCoinMax);

            if (increaseMoneyAbility.IsActiveAbility)
                randCoin += 5;
                
            _currentCoin += randCoin;
            SetCoinText();
        }

        public void DecreaseCoin(int count)
        {
            _currentCoin -= count;
            //AudioManager.Instance.PlaySfx("USECOIN");
            SetCoinText();
        }

        private void SetCoinText()
        {
            _coinText.text = $" : {_currentCoin}";
        }
    }
}