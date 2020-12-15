using Battles.Entities.Enemies;
using UnityEngine.AddressableAssets;

namespace AssetManagement
{
    public interface IAssetCache
    {
        AssetReference GetPlayerAsset();
        AssetReference GetEnemyAsset(EnemyType type);
        AssetReference GetProjectileAsset();
    }
}