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
            if (!_rawImage)
                _rawImage = GetComponent<RawImage>();

            var texture = Addressables.LoadAssetAsync<Texture2D>(_textureReference).WaitForCompletion();
            _rawImage.texture = texture;
        }
    }
}
