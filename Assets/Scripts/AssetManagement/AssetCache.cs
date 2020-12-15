using System;
using System.Linq;
using Battles.Entities.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetManagement
{
    [CreateAssetMenu(menuName = "SpaceInvaders/AssetsCache", fileName = "AssetsCache")]
    public class AssetCache : ScriptableObject, IAssetCache
    {
        [SerializeField]
        private AssetReference projectileAsset;

        [SerializeField]
        private AssetReference playerAsset;

        [SerializeField]
        private EnemyAsset[] enemyAssets;

        public AssetReference GetPlayerAsset() => playerAsset;
        public AssetReference GetProjectileAsset() => projectileAsset;

        public AssetReference GetEnemyAsset(EnemyType type)
        {
            return enemyAssets.First(x => x.type == type).asset;
        }

        [Serializable]
        private class EnemyAsset
        {
            public EnemyType type;
            public AssetReference asset;
        }
    }
}