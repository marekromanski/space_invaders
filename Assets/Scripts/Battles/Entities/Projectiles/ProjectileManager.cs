using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public class ProjectileManager : IDisposable
    {
        private readonly SignalBus signalBus;
        private readonly IProjectileFactory projectileFactory;
        private readonly IInstantiator instantiator;

        private readonly Dictionary<ProjectileDirection, Quaternion> directionToRotationMapping =
            new Dictionary<ProjectileDirection, Quaternion>
            {
                {ProjectileDirection.Down, Quaternion.Euler(0, 0, 0)},
                {ProjectileDirection.Up, Quaternion.Euler(0, 180, 0)}
            };

        [UsedImplicitly]
        public ProjectileManager(SignalBus signalBus, IProjectileFactory projectileFactory, IInstantiator instantiator)
        {
            this.signalBus = signalBus;
            this.projectileFactory = projectileFactory;
            this.instantiator = instantiator;

            signalBus.Subscribe<SpawnProjectileSignal>(OnSpawnProjectileSignal);
        }

        private void OnSpawnProjectileSignal(SpawnProjectileSignal signal)
        {
            var rotation = directionToRotationMapping[signal.direction];
            var projectile = projectileFactory.InstantiateProjectile(instantiator, signal.position, rotation);
            projectile.SetVelocity(signal.velocity, signal.direction);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<SpawnProjectileSignal>(OnSpawnProjectileSignal);
        }
    }
}