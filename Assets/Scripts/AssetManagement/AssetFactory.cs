using System;
using System.Collections.Generic;
using Battles;
using Battles.Entities;
using Battles.Entities.Enemies;
using Battles.Entities.Player;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using Zenject;

namespace AssetManagement
{
    public class AssetFactory : IEntitiesFactory
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
            var loadAssetTasks = StartLoadingEnemies();
            loadAssetTasks.Add(LoadAsset<GameObject>(assetCache.GetPlayerAsset()));

            await UniTask.WhenAll(loadAssetTasks);
        }

        private List<UniTask<GameObject>> StartLoadingEnemies()
        {
            var enemyTypes = (EnemyType[]) Enum.GetValues(typeof(EnemyType));
            var loadEnemiesTask = new List<UniTask<GameObject>>(enemyTypes.Length);
            foreach (var enemyType in enemyTypes)
            {
                loadEnemiesTask.Add(LoadAsset<GameObject>(assetCache.GetEnemyAsset(enemyType)));
            }

            return loadEnemiesTask;
        }

        private async UniTask<T> LoadAsset<T>(AssetReference asset)
        {
            Assert.IsNotNull(asset);
            var loadedAsset = await asset.LoadAssetAsync<T>().ToUniTask();
            loadedAssets.Add(asset.AssetGUID, loadedAsset);
            return loadedAsset;
        }

        public PlayerMb InstantiatePlayer(DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var playerObject = GetAsset<GameObject>(assetCache.GetPlayerAsset());

            return Instantiate<PlayerMb>(diContainer, playerObject, position, rotation, parent);
        }

        public EnemyMb InstantiateEnemy(EnemyType enemyType, DiContainer diContainer, Vector3 position,
            Quaternion rotation, Transform parent = null)
        {
            var enemyObject = GetAsset<GameObject>(assetCache.GetEnemyAsset(enemyType));

            return Instantiate<EnemyMb>(diContainer, enemyObject, position, rotation, parent);
        }

        private T Instantiate<T>(DiContainer diContainer, GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
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