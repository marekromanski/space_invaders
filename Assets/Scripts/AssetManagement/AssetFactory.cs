using System.Collections.Generic;
using Battles;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;

namespace AssetManagement
{
    public class AssetFactory : IPlayerFactory
    {
        private readonly IAssetCache assetCache;

        private readonly Dictionary<string, object> loadedAssets = new Dictionary<string, object>();

        [UsedImplicitly]
        public AssetFactory(IAssetCache assetCache)
        {
            this.assetCache = assetCache;
        }

        public async UniTask LoadAssets()
        {
            await LoadAsset<GameObject>(assetCache.PlayerAsset);
        }

        private async UniTask<T> LoadAsset<T>(AssetReference asset)
        {
            Assert.IsNotNull(asset);
            var loadedAsset = await asset.LoadAssetAsync<T>().ToUniTask();
            loadedAssets.Add(asset.AssetGUID, loadedAsset);
            return loadedAsset;
        }

        public Player InstantiatePlayer(DiContainer diContainer, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var playerObject = GetAsset<GameObject>(assetCache.PlayerAsset);

            var playerInstance = Instantiate<Player>(diContainer, playerObject, position, rotation, parent);
            return playerInstance;
        }

        private T Instantiate<T>(DiContainer diContainer, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T component = default;

            if (prefab != null && diContainer != null)
            {
                component = diContainer.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);
            }

            return component;
        }

        private T GetAsset<T>(AssetReference assetReference)
        {
            Assert.IsTrue(loadedAssets.Count > 0);
            return (T) loadedAssets[assetReference.AssetGUID];
        }
    }
}