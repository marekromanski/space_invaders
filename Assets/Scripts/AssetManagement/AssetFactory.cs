using System;
using System.Collections.Generic;
using Battles.Entities;
using Battles.Entities.Enemies;
using Battles.Entities.Player;
using Battles.Entities.Projectiles;
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
            var loadAssetTasks = LoadEnemies();
            loadAssetTasks.Add(LoadAsset<GameObject>(assetCache.GetPlayerAsset()));
            loadAssetTasks.Add(LoadAsset<GameObject>(assetCache.GetProjectileAsset()));

            await UniTask.WhenAll(loadAssetTasks);
        }

        public PlayerEntity InstantiatePlayer(IInstantiator instantiator, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var playerObject = GetAsset<GameObject>(assetCache.GetPlayerAsset());

            return Instantiate<PlayerEntity>(instantiator, playerObject, position, rotation, parent);
        }

        public EnemyMb InstantiateEnemy(EnemyType enemyType, IInstantiator instantiator, Vector3 position,
            Quaternion rotation, Transform parent = null)
        {
            var enemyObject = GetAsset<GameObject>(assetCache.GetEnemyAsset(enemyType));

            return Instantiate<EnemyMb>(instantiator, enemyObject, position, rotation, parent);
        }
        
        public ProjectileMb InstantiateProjectile(IInstantiator instantiator, Vector3 position,
            Quaternion rotation, Transform parent = null)
        {
            var projectileAsset = GetAsset<GameObject>(assetCache.GetProjectileAsset());

            return Instantiate<ProjectileMb>(instantiator, projectileAsset, position, rotation, parent);
        }


        private List<UniTask<GameObject>> LoadEnemies()
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

        private T Instantiate<T>(IInstantiator instantiator, GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            T component = default;

            if (prefab != null && instantiator != null)
            {
                component = instantiator.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);
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