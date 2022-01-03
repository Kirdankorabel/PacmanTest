using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    public class CoinsController : MonoBehaviour
    {
        public UnityEvent LaunchNextEnemy;
        public UnityEvent Wictory;
        private int _coinsCount = 0;
        private int _collectedCoinsCount = 0;
        private int _lounchEnemyPoints;

        public int CollectedCoinsCount => _collectedCoinsCount;
        public int CoinsCount
        {
            get => _coinsCount;

            set
            {
                if (_coinsCount == 0)
                    _coinsCount = value;
            }
        }

        private void Start()
        {
            GameController.Singletone.reset.AddListener(ResetCounters);
            GameController.Singletone.Player.takeCoin.AddListener(TakedCoin);
        }

        public void CalclounchEnemyPoints()
            => _lounchEnemyPoints = GameController.Singletone.CoinsController.CoinsCount / (StaticDate.Settings.EnemiesNumber + 1);

        private void TakedCoin()
        {
            _collectedCoinsCount++;
            if (_collectedCoinsCount / _lounchEnemyPoints == GameController.Singletone.EnemyController.ActiveEnemyCount
               && _collectedCoinsCount != 0)
            {
                LaunchNextEnemy?.Invoke();
            }

            if (_collectedCoinsCount == _coinsCount)
                Wictory?.Invoke();
        }

        private void ResetCounters()
        {
            _coinsCount = 0;
            _collectedCoinsCount = 0;
        }
    }
}
