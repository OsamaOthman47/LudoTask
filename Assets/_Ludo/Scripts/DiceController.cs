using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class DiceController : MonoBehaviour
    {
        [SerializeField] Button _diceButton;
        [SerializeField] Transform[] _dices;
        [SerializeField] float _frameRate = 0.1f;

        int currentFrame;
        float timer;
        bool _isRolling;

        private void OnEnable()
        {
            _diceButton.onClick.AddListener(OnDiceClicked);
        }

        private void OnDisable()
        {
            _diceButton.onClick.RemoveAllListeners();
        }

        private void OnDiceClicked()
        {
            _isRolling = true;
        }

        private void Update()
        {
            if (!_isRolling) return;

            timer += Time.deltaTime;

            if (timer >= _frameRate)
            {
                timer -= _frameRate;
                currentFrame = (currentFrame + 1) % _dices.Length;
                _dices[currentFrame].SetAsLastSibling();
            }
        }
    }
}
