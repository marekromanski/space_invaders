using System.Collections;
using Battles.Entities.Projectiles;
using Battles.Mechanics.Shooting;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemyEntity : MonoBehaviour
    {
        [SerializeField]
        private Transform projectileSpawnPosition;

        private SignalBus signalBus;
        private IEnemiesConfiguration enemiesConfiguration;
        private ShootingComponent shootingomponent;

        private EnemyType type;
        
        private void Awake()
        {
            Assert.IsNotNull(projectileSpawnPosition);
        }

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IEnemiesConfiguration enemiesConfiguration)
        {
            this.signalBus = signalBus;
            this.enemiesConfiguration = enemiesConfiguration;
        }

        private void AddShootingComponent()
        {
            var enemyConfiguration = enemiesConfiguration.GetEnemyConfiguration(type);
            var shootingParameters = new ShootingParameters(enemyConfiguration.ProjectileVelocity,
                enemyConfiguration.MinShootingInterval, projectileSpawnPosition, ProjectileDirection.Down,
                enemyConfiguration.ProjectileLifetime);
            shootingomponent = new ShootingComponent(shootingParameters, signalBus);
        }

        public void Init(EnemyType enemyType)
        {
            type = enemyType;
            AddShootingComponent();

            StartCoroutine(ShootingCoroutine());
        }

        private IEnumerator ShootingCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.3f);
                shootingomponent.AttemptShot();
            }
        }
    }
}