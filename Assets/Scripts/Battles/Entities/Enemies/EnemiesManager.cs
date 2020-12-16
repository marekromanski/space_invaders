using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemiesManager : IDisposable
    {
        private readonly SignalBus signalBus;

        private readonly Dictionary<EnemyType, List<EnemyEntity>> activeEnemies =
            new Dictionary<EnemyType, List<EnemyEntity>>
            {
                {EnemyType.MotherShip, new List<EnemyEntity>()},
                {EnemyType.Elite, new List<EnemyEntity>()},
                {EnemyType.Regular, new List<EnemyEntity>()},
            };


        [UsedImplicitly]
        public EnemiesManager(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            signalBus.Subscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private void OnEnemySpawned(EnemySpawnedSignal signal)
        {
            activeEnemies[signal.type].Add(signal.entity);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            activeEnemies[signal.type].Remove(signal.entity);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }
    }
}