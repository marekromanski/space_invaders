using AssetManagement;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public class ProjectileSpawner : PoolingSpawner<ProjectileMb>, IProjectileSpawner
    {
        private readonly IProjectileFactory projectileFactory;
        private readonly IInstantiator instantiator;
        private readonly SignalBus signalBus;

        [UsedImplicitly]
        public ProjectileSpawner(IInstantiator instantiator, IProjectileFactory projectileFactory, SignalBus signalBus)
        {
            this.projectileFactory = projectileFactory;
            this.instantiator = instantiator;
            this.signalBus = signalBus;

            SubscribeToDestroySignal();
        }

        protected override ProjectileMb SpawnNewInstance(Vector3 position, Quaternion rotation)
        {
            return projectileFactory.InstantiateProjectile(instantiator, position, rotation);
        }

        protected override void SubscribeToDestroySignal()
        {
            signalBus.Subscribe<ProjectileDestroyedSignal>(OnProjectileDestroyed);
        }

        private void OnProjectileDestroyed(ProjectileDestroyedSignal signal)
        {
            signal.projectile.OnDespawned();
            objectPool.Push(signal.projectile);
        }

        protected override void UnsubscribeToDestroySignal()
        {
            signalBus.Unsubscribe<ProjectileDestroyedSignal>(OnProjectileDestroyed);
        }
    }
}