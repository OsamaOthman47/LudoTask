using System.Collections;
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

        private void OnEnable()
        {
            _diceButton.onClick.AddListener(OnDiceClicked);
        }

        private void OnDisable()
        {
            _diceButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (!GameController.Instance.IsDiceRolling) return;

            timer += Time.deltaTime;

            if (timer >= _frameRate)
            {
                timer -= _frameRate;
                currentFrame = (currentFrame + 1) % _dices.Length;
                _dices[currentFrame].SetAsLastSibling();
            }
        }

        private void OnDiceClicked()
        {
            _diceButton.interactable = false;
            GetRandomNumberAPI();
        }

        public void GetRandomNumberAPI()
        {
            GameController.Instance.IsDiceRolling = true;
            string url = Constants.GET_GENERATE_RANDOM_NUMBER;
            APIServices.Instance.HandleAPIServices(url, OnGetRandomNumberResponse);
        }

        private void OnGetRandomNumberResponse(bool isSuccess, string response)
        {
            if (isSuccess)
            {
                if (int.TryParse(response, out int value))
                {
                    StartCoroutine(ExecuteAfterTime(0.5f, value));
                }
                else
                {
                    Debug.LogError("Invalid Value");
                    ResetDice();
                }
            }
            else
            {
                Debug.LogError(response);
            }
        }

        private IEnumerator ExecuteAfterTime(float time, int value)
        {
            // Wait for the specified time
            yield return new WaitForSeconds(time);

            // Execute your code here after the delay
            SetNumberOnDice(value);
        }

        private void SetNumberOnDice(int number)
        {
            Debug.Log(number);
            ResetDice();
            _dices[number - 1].SetAsLastSibling();
            GameController.Instance.CurrentDiceValue = number;
        }

        private void ResetDice()
        {
            _diceButton.interactable = true;
            GameController.Instance.IsDiceRolling = false;
        }
    }
}
