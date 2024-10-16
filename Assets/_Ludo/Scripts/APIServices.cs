using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Ludo
{
    public class APIServices : MonoBehaviour
    {
        public static APIServices Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void HandleAPIServices(string url, Action<bool, string> callback)
        {
            StartCoroutine(SendRequest(url, callback));
        }

        private IEnumerator SendRequest(string url, Action<bool, string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("User-Agent", "UnityWebRequest");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                callback(true, response);
            }
            else
            {
                string error = request.error;
                callback(false, error);
            }
        }
    }
}
