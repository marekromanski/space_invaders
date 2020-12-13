using System;
using System.Collections.Generic;
using Battles;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

namespace AssetManagement
{
    public class AssetFactory
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

        public async UniTask<T> LoadAsset<T>(AssetReference asset)
        {
            Assert.IsNotNull(asset);
            var loadedAsset = await asset.LoadAssetAsync<T>().ToUniTask();
            loadedAssets.Add(asset.AssetGUID, loadedAsset);
            return loadedAsset;
        }

        public T GetAsset<T>(AssetReference assetReference)
        {
            Assert.IsTrue(loadedAssets.Count > 0);
            return (T) loadedAssets[assetReference.AssetGUID];
        }
    }
}