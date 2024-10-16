using UnityEngine;

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

        private void Awake()
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
    }

    public enum PlayerColor
    {
        Blue, Green, Red, Yellow,
    }
}
