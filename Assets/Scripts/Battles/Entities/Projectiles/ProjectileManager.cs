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
        private readonly IProjectileSpawner projectileSpawner;


        private readonly Dictionary<ProjectileDirection, Quaternion> directionToRotationMapping =
            new Dictionary<ProjectileDirection, Quaternion>
            {
                {ProjectileDirection.Down, Quaternion.Euler(90, 0, 0)},
                {ProjectileDirection.Up, Quaternion.Euler(-90, 180, 0)}
            };

        private readonly List<ProjectileMb> activeProjectiles = new List<ProjectileMb>();


        [UsedImplicitly]
        public ProjectileManager(SignalBus signalBus, IProjectileSpawner projectileSpawner)
        {
            this.signalBus = signalBus;
            this.projectileSpawner = projectileSpawner;

            signalBus.Subscribe<SpawnProjectileSignal>(OnSpawnProjectileSignal);
        }

        private void OnSpawnProjectileSignal(SpawnProjectileSignal signal)
        {
            var rotation = directionToRotationMapping[signal.direction];
            var projectile = projectileSpawner.Spawn(signal.position, rotation);
            projectile.SetCharacteristics(signal.velocity, signal.direction, signal.lifeTime);

            activeProjectiles.Add(projectile);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<SpawnProjectileSignal>(OnSpawnProjectileSignal);
        }

        public void Tick()
        {
            CheckProjectilesLifeTime();
            MoveProjectiles();
        }

        private void CheckProjectilesLifeTime()
        {
            for (var i = activeProjectiles.Count - 1; i >= 0; --i)
            {
                var projectile = activeProjectiles[i];
                if (projectile.TimeElapsed())
                {
                    activeProjectiles.RemoveAt(i);
                    signalBus.Fire(new ProjectileDestroyedSignal(projectile));
                }
            }
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