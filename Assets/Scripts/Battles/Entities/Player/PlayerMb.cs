using Battles.Entities.Projectiles;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Battles.Entities.Player
{
    public class PlayerMb : MonoBehaviour
    {
        [SerializeField]
        private Transform projectileSpawnPosition;
        
        private SignalBus signalBus;
        private IPlayerConfiguration playerConfiguration;

        private void Awake()
        {
            Assert.IsNotNull(projectileSpawnPosition);
        }

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IPlayerConfiguration playerConfiguration)
        {
            this.signalBus = signalBus;
            this.playerConfiguration = playerConfiguration;

            signalBus.Subscribe<PlayerMovedSignal>(OnPlayerMoved);
            signalBus.Subscribe<PlayerShotSignal>(OnPlayerShot);
        }

        private void OnPlayerShot()
        {
            var velocity = playerConfiguration.ProjectileVelocity;
            signalBus.Fire(new SpawnProjectileSignal(projectileSpawnPosition.position, velocity, ProjectileDirection.Up));
        }


        private void OnPlayerMoved(PlayerMovedSignal signal)
        {
            var currentPosition = transform.position;
            var targetPosition = new Vector3(currentPosition.x + signal.delta, currentPosition.y, currentPosition.z);

            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * playerConfiguration.MoveSpeed);
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<PlayerMovedSignal>(OnPlayerMoved);
            signalBus.Unsubscribe<PlayerShotSignal>(OnPlayerShot);
        }
    }
}