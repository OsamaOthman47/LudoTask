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

        private void Awake()
        {
            _button = GetComponent<Button>();
            SelectPath();
        }

        private void SelectPath()
        {
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
            if (GameController.Instance.CurrentDiceValue.Equals(0)) return;
            if (GameController.Instance.IsPlayerMoving) return;

            StartCoroutine(MovePlayer());
        }

        private IEnumerator MovePlayer()
        {
            GameController.Instance.IsPlayerMoving = true;

            Vector3 initialScale = transform.localScale;   // Store the original scale
            Vector3 scaledUp = initialScale * 1.5f;        // Slightly larger scale

            for (int i = 1; i <= GameController.Instance.CurrentDiceValue; i++)
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = path[i - 1].position;
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

                yield return new WaitForSeconds(0.2f);

                // Ensure the final position and scale are set correctly
                transform.position = targetPosition;
                transform.localScale = initialScale;
            }

            GameController.Instance.IsPlayerMoving = false;
            GameController.Instance.CurrentDiceValue = 0;
        }
    }
}
