using System;
using JetBrains.Annotations;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemiesManager : IDisposable
    {
        private readonly SignalBus signalBus;

        [UsedImplicitly]
        public EnemiesManager(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            signalBus.Subscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private void OnEnemySpawned()
        {
            throw new NotImplementedException();
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }
    }
}