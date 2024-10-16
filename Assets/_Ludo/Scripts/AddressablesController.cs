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

        public IEnumerator DownloadAddressables()
        {
            AsyncOperationHandle asyncOperationHandle = Addressables.DownloadDependenciesAsync(_label);
            while (!asyncOperationHandle.IsDone)
            {
                float downloadProgress = asyncOperationHandle.GetDownloadStatus().Percent;

                if ((int)asyncOperationHandle.GetDownloadStatus().TotalBytes != 0)
                {
                    Debug.Log("Addressables Download Progress: " + downloadProgress);
                }

                yield return null;
            }

            _loaderGO.SetActive(false);

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Downloaded Addressables successfully.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.LogError("Failed to download Addressables: " + asyncOperationHandle.OperationException.Message);
            }
        }
    }
    public enum AddressableType
    {
        Texture,
    }
}
