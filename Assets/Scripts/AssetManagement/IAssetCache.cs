using Battles.Entities;
using UnityEngine.AddressableAssets;

namespace AssetManagement
{
    public interface IAssetCache
    {
        AssetReference GetPlayerAsset();
        AssetReference GetEnemyAsset(EnemyType type);
    }
}