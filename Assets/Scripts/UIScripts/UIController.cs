using UnityEngine;

namespace UIScripts
{
    public class UIController : MonoBehaviour
    {
        public static UIController Singletone { get; set; }
        [SerializeField] private GameUI _gamePanel;
        [SerializeField] private ResultUI _resultPanel;
        [SerializeField] private MenuUI _menuPanel;

        public GameUI GamePanel => _gamePanel;
        public MenuUI MenuPanel => _menuPanel;

        private void Awake()
        {
            StorageManager.LoadParams();
            StorageManager.SaveParams();
            Singletone = this;
        }

        private void Start()
        {
            GameController.Singletone.gameOver.AddListener(ShowResults);
        }

        private void ShowResults()
        {
            _resultPanel.gameObject.SetActive(true);
            _gamePanel.gameObject.SetActive(false);
        }
    }
}
