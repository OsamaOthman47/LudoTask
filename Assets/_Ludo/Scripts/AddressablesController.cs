using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Ludo
{
    public class AddressablesController : MonoBehaviour
    {
        [SerializeField] GameObject _loaderGO;
        [SerializeField] AssetLabelReference _label;

        private void Awake()
        {
            StartCoroutine(DownloadAddressables());
        }

        /// <summary>
        /// Downloads addressable assets asynchronously using the specified label.
        /// Tracks the download progress and logs it. 
        /// Upon completion, if the download is successful, it loads the next scene. 
        /// If the download fails, logs an error message.
        /// </summary>
        public IEnumerator DownloadAddressables()
        {
            AsyncOperationHandle asyncOperationHandle = Addressables.DownloadDependenciesAsync(_label);
            while (!asyncOperationHandle.IsDone)
            {
                float downloadProgress = asyncOperationHandle.GetDownloadStatus().Percent;

                if ((int)asyncOperationHandle.GetDownloadStatus().TotalBytes != 0)
                {
                    Debug.Log("Addressables Assets Downloading Progress: " + downloadProgress);
                }

                yield return null;
            }

            _loaderGO.SetActive(false);

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressables Assets Downloaded Successfully.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.LogError(asyncOperationHandle.OperationException.Message);
            }
        }
    }
    public enum AddressableType
    {
        Texture,
    }
}
