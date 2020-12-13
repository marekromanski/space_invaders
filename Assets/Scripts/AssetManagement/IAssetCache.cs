using UnityEngine.AddressableAssets;

namespace AssetManagement
{
    public interface IAssetCache
    {
        AssetReference PlayerAsset { get; }
    }
}