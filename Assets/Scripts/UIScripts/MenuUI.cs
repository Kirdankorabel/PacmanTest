using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _resultsButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _startButton.onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            GameController.Singletone.StartNewLevel();
            this.gameObject.SetActive(false);
            UIController.Singletone.GamePanel.gameObject.SetActive(true);
        }
    }
}
