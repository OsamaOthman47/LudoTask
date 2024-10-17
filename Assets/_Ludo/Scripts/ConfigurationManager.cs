using UnityEngine;

namespace Ludo
{
    public class ConfigurationManager : MonoBehaviour
    {
        private void Awake()
        {
            // set the target frame rate for the game based on the current screen refresh rate
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        }
    }
}
