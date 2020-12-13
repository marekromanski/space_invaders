using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetManagement
{
    [CreateAssetMenu(menuName = "SpaceInvaders/AssetsCache", fileName = "AssetsCache")]

    public class AssetCache : ScriptableObject, IAssetCache
    {
        [SerializeField]
        private AssetReference playerAsset;

        public AssetReference PlayerAsset => playerAsset;
    }

    public interface IAssetCache
    {
        AssetReference PlayerAsset { get; }
    }
}