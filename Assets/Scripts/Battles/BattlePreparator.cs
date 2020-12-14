using Battles.Entities;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles
{
    public class BattlePreparator : MonoBehaviour
    {
        [SerializeField]
        private Transform playerSpawnPosition;

        [SerializeField]
        private Transform enemySpawnPosition;

        private IEntitiesFactory factory;
        private DiContainer diContainer;

        [Inject, UsedImplicitly]
        private void Construct(IEntitiesFactory factory, DiContainer diContainer)
        { 
            this.factory = factory;
            this.diContainer = diContainer;
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            factory.InstantiateEnemy(EnemyType.MotherShip, diContainer, enemySpawnPosition.position, Quaternion.identity);
        }

        private void SpawnPlayer()
        {
            factory.InstantiatePlayer(diContainer, playerSpawnPosition.position, Quaternion.identity);
        }
    }
}