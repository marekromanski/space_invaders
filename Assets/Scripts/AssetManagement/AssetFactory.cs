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

        private readonly Dictionary<Type, object> loadedAssets = new Dictionary<Type, object>();

        [UsedImplicitly]
        public AssetFactory(IAssetCache assetCache)
        {
            this.assetCache = assetCache;
        }

        public async UniTask LoadAssets()
        {
            Debug.Log("Loading assets started");
            var loadPlayerTask = await LoadAsset<GameObject>(assetCache.PlayerAsset);
        }

        public async UniTask<T> LoadAsset<T>(AssetReference asset)
        {
            Assert.IsNotNull(asset);
            var loadedAsset = await asset.LoadAssetAsync<T>().ToUniTask();
            Debug.Log("Asset loaded");
            loadedAssets.Add(typeof(T), loadedAsset);
            Debug.Log("asset saved");
            return loadedAsset;
        }

        public T GetAsset<T>()
        {
            Assert.IsTrue(loadedAssets.Count > 0);
            return (T) loadedAssets[typeof(T)];
        }
    }
}