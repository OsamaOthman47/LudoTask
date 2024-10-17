using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerColor playerColor;
        Button _button;
        Transform[] path;
        int currentPlace;

        private void Awake()
        {
            _button = GetComponent<Button>();
            SelectPath();
        }

        private void SelectPath()
        {
            // select each path relative to every player's color

            switch (playerColor)
            {
                case PlayerColor.Blue:
                    var targetPath = GameController.Instance.bluePath;
                    path = new Transform[targetPath.Length];
                    path = targetPath;
                    break;

                default:
                    Debug.LogError("Unknown color.");
                    break;
            }
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnPlayerClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void OnPlayerClicked()
        {
            // Don't proceed if the dice is rolling or the player is moving or at the start of the game
            if (GameController.Instance.CurrentDiceValue.Equals(0)) return;
            if (GameController.Instance.IsPlayerMoving) return;
            if (GameController.Instance.IsDiceRolling) return;

            StartCoroutine(MovePlayer()); // to move player with smooth animation
        }

        private IEnumerator MovePlayer()
        {
            GameController.Instance.ClearHintText();
            GameController.Instance.IsPlayerMoving = true;
            Vector3 initialScale = transform.localScale;   // Store the original scale
            Vector3 scaledUp = initialScale * 1.5f;        // Slightly larger scale
            int loopValue = GameController.Instance.CurrentDiceValue += currentPlace; // this value is relative to the current player place and the current value of the dice
            int counter = 0;

            GameController.Instance.diceButton.interactable = false; // don't allow to click roll dice while player moving
            GameController.Instance.HintText("Moving...");

            for (int i = currentPlace; i < loopValue; i++)
            {
                if (loopValue > path.Length) // check if the current dice value is not more than the path length
                {
                    int leftValue = path.Length - currentPlace; // the exact value to reach the end of the path 
                    GameController.Instance.HintText($"The dice value is more than the path limit, you need to get {leftValue}. Roll the dice again or reset the game.");
                    ResetValues();
                    yield break;  // Exit and stop the coroutine
                }

                counter++;
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = path[i].position;
                float duration = 0.3f; // Time to move between positions
                float elapsedTime = 0f;

                // Smoothly move towards the target position over time
                while (elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

                    // Interpolate scale (scale up in the first half of the movement, and down in the second half)
                    if (elapsedTime < duration / 2)
                    {
                        // Scale up during the first half
                        transform.localScale = Vector3.Lerp(initialScale, scaledUp, elapsedTime / (duration / 2));
                    }
                    else
                    {
                        // Scale back down during the second half
                        transform.localScale = Vector3.Lerp(scaledUp, initialScale, (elapsedTime - duration / 2) / (duration / 2));
                    }

                    elapsedTime += Time.deltaTime; // Increment the time
                    yield return null; // Wait until the next frame
                }

                yield return new WaitForSeconds(0.2f); // time to pause between the moves

                // Ensure the final position and scale are set correctly
                transform.position = targetPosition;
                transform.localScale = initialScale;
            }

            currentPlace += counter; // cache the current place that the player has reached
            ResetValues();


            if (loopValue.Equals(path.Length)) //Check if the player reached the end of the path
            {
                GameController.Instance.HintText("The player has reached the path limit, Press the reset button");
                GameController.Instance.diceButton.interactable = false;
            }
            else 
            {
                GameController.Instance.HintText("Roll the dice.");
            }
        }

        private void ResetValues()
        {
            GameController.Instance.IsPlayerMoving = false;
            GameController.Instance.CurrentDiceValue = 0;
            GameController.Instance.diceButton.interactable = true;
        }
    }
}
