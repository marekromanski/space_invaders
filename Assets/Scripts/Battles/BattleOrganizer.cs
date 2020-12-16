using Battles.Entities;
using Battles.Entities.Enemies;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles
{
    public class BattleOrganizer : MonoBehaviour
    {
        [SerializeField]
        private Transform playerSpawnPosition;

        [SerializeField]
        private Transform enemySpawnPosition;

        private IEntitiesFactory factory;
        private DiContainer diContainer;
        private IMothershipSpawner mothershipSpawner;
        private IEliteEnemySpawner eliteEnemySpawner;
        private IRegularEnemySpawner regularEnemySpawner;
        private SignalBus signalBus;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IEntitiesFactory factory, DiContainer diContainer,
            IMothershipSpawner mothershipSpawner, IEliteEnemySpawner eliteEnemySpawner, IRegularEnemySpawner regularEnemySpawner)
        {
            this.signalBus = signalBus;
            this.factory = factory;
            this.diContainer = diContainer;

            this.mothershipSpawner = mothershipSpawner;
            this.eliteEnemySpawner = eliteEnemySpawner;
            this.regularEnemySpawner = regularEnemySpawner;
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            var enemy = mothershipSpawner.Spawn(enemySpawnPosition.position, Quaternion.identity);
            signalBus.Fire(new EnemySpawnedSignal(EnemyType.MotherShip, enemy));
        }

        private void SpawnPlayer()
        {
            factory.InstantiatePlayer(diContainer, playerSpawnPosition.position, Quaternion.identity);
        }
    }
}