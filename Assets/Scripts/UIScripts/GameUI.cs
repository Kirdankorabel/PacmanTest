using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Text _pointsCounterText;
        [SerializeField] private List<Image> _healthImages;

        private void Update()
        {
            _pointsCounterText.text = GameController.Singletone.Points.ToString();
        }

        private void Start()
        {
            GameController.Singletone.Player.playerCaught.AddListener(UpdateHealthCount);
        }

        private void UpdateHealthCount()
            => _healthImages[GameController.Singletone.HealthCounter].gameObject.SetActive(false);
    }
}
