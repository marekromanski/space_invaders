using Battles.Entities.Projectiles;
using Battles.Mechanics.Shooting;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Battles.Entities.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField]
        private Transform projectileSpawnPosition;

        private SignalBus signalBus;
        private ICharacterConfiguration playerConfiguration;
        private ShootingComponent shootingomponent;

        private void Awake()
        {
            Assert.IsNotNull(projectileSpawnPosition);
        }

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, ICharacterConfiguration playerConfiguration)
        {
            this.signalBus = signalBus;
            this.playerConfiguration = playerConfiguration;

            AddShootingComponent();

            signalBus.Subscribe<PlayerMovedSignal>(OnPlayerMoved);
        }

        private void AddShootingComponent()
        {
            var shootingParameters = new ShootingParameters(playerConfiguration.ProjectileVelocity,
                playerConfiguration.MaxShootingFrequency, projectileSpawnPosition, ProjectileDirection.Up,
                playerConfiguration.ProjectileLifetime);
            shootingomponent = new ShootingComponent(shootingParameters, signalBus);
        }

        private void OnPlayerMoved(PlayerMovedSignal signal)
        {
            var currentPosition = transform.position;
            var targetPosition = new Vector3(currentPosition.x + signal.delta, currentPosition.y, currentPosition.z);

            transform.position = Vector3.Lerp(currentPosition, targetPosition,
                Time.deltaTime * playerConfiguration.MoveSpeed);
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<PlayerMovedSignal>(OnPlayerMoved);
            shootingomponent.Dispose();
        }
    }
}