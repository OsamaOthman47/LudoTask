using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ludo
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        public Transform[] bluePath;
        public Transform[] greenPath;
        public Transform[] redPath;
        public Transform[] yellowPath;

        public int CurrentDiceValue { set; get; }
        public bool IsPlayerMoving { set; get; }
        public bool IsDiceRolling { set; get; }

        public Button diceButton;

        [SerializeField] TextMeshProUGUI _hintText;
        [SerializeField] Button _resetButton;

        private void Awake() // create singleton
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            HintText("Roll the dice.");
            _resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        public void HintText(string hint)
        {
            _hintText.text = hint;
        }

        public void ClearHintText()
        {
            _hintText.text = string.Empty;
        }

        private void OnResetButtonClicked()
        {
            // reset game scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public enum PlayerColor
    {
        Blue, Green, Red, Yellow,
    }
}
