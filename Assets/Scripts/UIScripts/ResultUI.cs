using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private Text _resultText;
        [SerializeField] private Button _acceptButton;

        private void Start()
        {
            this.gameObject.SetActive(true);
            _resultText.text = GameController.Singletone.Points.ToString();
            _acceptButton.onClick.AddListener(ExitToMenu);
        }

        private void ExitToMenu()
        {
            UIController.Singletone.MenuPanel.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            GameController.Singletone.ResetAccount();
        }
    }
}
