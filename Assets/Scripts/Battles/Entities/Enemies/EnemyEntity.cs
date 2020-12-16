using Battles.Entities.Projectiles;
using Battles.Mechanics.Shooting;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemyEntity : MonoBehaviour, IPoolable
    {
        private const string ProjectileTag = "Projectile";

        [SerializeField]
        private Transform projectileSpawnPosition;

        private SignalBus signalBus;
        private IEnemiesConfiguration enemiesConfiguration;
        private ShootingComponent shootingomponent;

        private EnemyType type;

        public Vector3 ProjectileSpawnPosition => projectileSpawnPosition.position; 

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
        }

        public void AttemptShot()
        {
            shootingomponent.AttemptShot();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Tags.PROJECTILE))
            {
                signalBus.Fire(new EnemyDestroyedSignal(type, this));
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }
    }
}