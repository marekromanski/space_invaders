using AssetManagement;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class MotherShipSpawner : EnemySpawner, IMothershipSpawner
    {
        [UsedImplicitly]
        public MotherShipSpawner(IInstantiator instantiator, IEnemiesFactory enemiesFactory, SignalBus signalBus) :
            base(EnemyType.MotherShip, instantiator, enemiesFactory, signalBus)
        {
        }
    }

    public class EliteEnemySpawner : EnemySpawner, IEliteEnemySpawner
    {
        [UsedImplicitly]
        public EliteEnemySpawner(IInstantiator instantiator, IEnemiesFactory enemiesFactory, SignalBus signalBus) :
            base(EnemyType.Elite, instantiator, enemiesFactory, signalBus)
        {
        }
    }

    public class RegularEnemySpawner : EnemySpawner, IRegularEnemySpawner
    {
        [UsedImplicitly]
        public RegularEnemySpawner(IInstantiator instantiator, IEnemiesFactory enemiesFactory, SignalBus signalBus) :
            base(EnemyType.Regular, instantiator, enemiesFactory, signalBus)
        {
        }
    }

    public abstract class EnemySpawner : PoolingSpawner<EnemyEntity>, IEnemySpawner
    {
        private readonly IEnemiesFactory enemiesFactory;
        private readonly IInstantiator instantiator;
        private readonly SignalBus signalBus;

        private readonly EnemyType handledType;

        protected EnemySpawner(EnemyType handledType, IInstantiator instantiator, IEnemiesFactory enemiesFactory,
            SignalBus signalBus)
        {
            this.enemiesFactory = enemiesFactory;
            this.instantiator = instantiator;
            this.signalBus = signalBus;
            this.handledType = handledType;

            SubscribeToDestroySignal();
        }

        protected override EnemyEntity SpawnNewInstance(Vector3 position, Quaternion rotation)
        {
            var enemy = enemiesFactory.InstantiateEnemy(handledType, instantiator, position, rotation);
            enemy.Init(handledType);
            return enemy;
        }

        protected override void SubscribeToDestroySignal()
        {
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            if (signal.type == handledType)
            {
                signal.entity.OnDespawned();
                objectPool.Push(signal.entity);
            }
        }

        protected override void UnsubscribeToDestroySignal()
        {
            signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }
    }
}