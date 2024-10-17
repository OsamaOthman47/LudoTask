using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Ludo
{
    public class AddressableLoader : MonoBehaviour
    {
        [SerializeField] AddressableType _addressableType;
        [SerializeField] AssetReferenceTexture _textureReference;
        RawImage _rawImage;

        private void Awake()
        {
            // handle the type of the asset e.g. texture

            switch (_addressableType)
            {
                case AddressableType.Texture:
                    HandleTexture();
                    break;

                default:
                    Debug.LogError("Unknown type.");
                    break;
            }
        }

        private void HandleTexture()
        {
            _rawImage = GetComponent<RawImage>();
            var texture = Addressables.LoadAssetAsync<Texture2D>(_textureReference).WaitForCompletion(); // load texture based on the reference and wait for completion is used so the function ends at the same frame
            _rawImage.texture = texture;
        }
    }
}
