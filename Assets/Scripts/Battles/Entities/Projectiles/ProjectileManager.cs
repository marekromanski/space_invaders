using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public class ProjectileManager : IDisposable, ITickable
    {
        private readonly SignalBus signalBus;
        private readonly IProjectileFactory projectileFactory;
        private readonly IInstantiator instantiator;

        private readonly Dictionary<ProjectileDirection, Quaternion> directionToRotationMapping =
            new Dictionary<ProjectileDirection, Quaternion>
            {
                {ProjectileDirection.Down, Quaternion.Euler(90, 0, 0)},
                {ProjectileDirection.Up, Quaternion.Euler(-90, 180, 0)}
            };

        private readonly List<ProjectileMb> activeProjectiles = new List<ProjectileMb>();
        private readonly Stack<ProjectileMb> projectilePool = new Stack<ProjectileMb>();

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
            var projectile = InstantiateProjectile(signal.position, rotation);
            projectile.SetVelocity(signal.velocity, signal.direction);

            activeProjectiles.Add(projectile);
        }

        private ProjectileMb InstantiateProjectile(Vector3 position, Quaternion rotation)
        {
            if (projectilePool.Count > 0)
            {
                var projectile = projectilePool.Pop();
                projectile.transform.SetPositionAndRotation(position, rotation);
                projectile.OnSpawned();
                return projectile;
            }

            return projectileFactory.InstantiateProjectile(instantiator, position, rotation);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<SpawnProjectileSignal>(OnSpawnProjectileSignal);
        }

        public void Tick()
        {
            MoveProjectiles();
        }

        private void MoveProjectiles()
        {
            foreach (var projectile in activeProjectiles)
            {
                projectile.Move();
            }
        }
    }
}