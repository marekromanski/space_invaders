using System.Collections.Generic;
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

        private readonly Dictionary<EnemyType, IEnemySpawner> enemySpawners = new Dictionary<EnemyType, IEnemySpawner>(3);
         
        private SignalBus signalBus;
        private IBattleConfig battleConfig;

        [Inject, UsedImplicitly]
        private void Construct(IBattleConfig battleConfig, SignalBus signalBus, IEntitiesFactory factory, DiContainer diContainer,
            IMothershipSpawner mothershipSpawner, IEliteEnemySpawner eliteEnemySpawner, IRegularEnemySpawner regularEnemySpawner)
        {
            this.battleConfig = battleConfig;
            this.signalBus = signalBus;
            this.factory = factory;
            this.diContainer = diContainer;
            
            enemySpawners.Add(EnemyType.MotherShip, mothershipSpawner);
            enemySpawners.Add(EnemyType.Elite, eliteEnemySpawner);
            enemySpawners.Add(EnemyType.Regular, regularEnemySpawner);
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnWave();
        }

        private void SpawnWave()
        {
            SpawnMotherships();
            SpawnElites();
            SpawnRegulars();

        }

        private void SpawnMotherships()
        {
            for (int i = 0; i < battleConfig.GetAmountOf(EnemyType.MotherShip); ++i)
            {
                var position = GetSpawnPosition(EnemyType.MotherShip);
                SpawnEnemy(EnemyType.MotherShip, position);
            }
        }

        private void SpawnElites()
        {
            for (int i = 0; i < battleConfig.GetAmountOf(EnemyType.Elite); ++i)
            {
                var position = GetSpawnPosition(EnemyType.Elite);
                SpawnEnemy(EnemyType.Elite, position);
            }
        }

        private void SpawnRegulars()
        {
            for(int row = 0; row < battleConfig.GetAmountOfRegularRows(); ++row)
            {
                for (int i = 0; i < battleConfig.GetAmountOf(EnemyType.Regular); ++i)
                {
                    var position = GetSpawnPosition(EnemyType.Regular);
                    SpawnEnemy(EnemyType.Regular, position);
                }
            }
        }

        private Vector3 GetSpawnPosition(EnemyType enemyType)
        {
            return enemySpawnPosition.position;
        }

        private void SpawnEnemy(EnemyType enemyType, Vector3 position)
        {
            var enemy = enemySpawners[enemyType].Spawn(position, Quaternion.identity);
            signalBus.Fire(new EnemySpawnedSignal(enemyType, enemy));
        }

        private void SpawnPlayer()
        {
            factory.InstantiatePlayer(diContainer, playerSpawnPosition.position, Quaternion.identity);
        }
    }
}