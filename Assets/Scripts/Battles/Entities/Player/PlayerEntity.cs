using System;
using Battles.Entities.Projectiles;
using Battles.Mechanics.Shooting;
using Cysharp.Threading.Tasks;
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

        [SerializeField]
        private Material transparentMaterial;

        private SignalBus signalBus;
        private IPlayerConfiguration playerConfiguration;
        private ShootingComponent shootingomponent;

        private int livesRemaining;
        private bool isInvulnerabilityEnabled;

        private void Awake()
        {
            Assert.IsNotNull(projectileSpawnPosition);
        }

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IPlayerConfiguration playerConfiguration)
        {
            this.signalBus = signalBus;
            this.playerConfiguration = playerConfiguration;

            livesRemaining = playerConfiguration.LivesTotal;

            AddShootingComponent();

            signalBus.Subscribe<ShotAttemptSignal>(OnShotAttempt);
            signalBus.Subscribe<PlayerMovedSignal>(OnPlayerMoved);
        }

        private void OnShotAttempt()
        {
            shootingomponent.AttemptShot();
        }

        private void AddShootingComponent()
        {
            var shootingParameters = new ShootingParameters(playerConfiguration.ProjectileVelocity,
                playerConfiguration.MinShootingInterval, projectileSpawnPosition, ProjectileDirection.Up,
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

        private void OnCollisionEnter(Collision other)
        {
            if (CanBeDamaged())
            {
                if (other.gameObject.CompareTag(Tags.PROJECTILE) || other.gameObject.CompareTag(Tags.ENEMY))
                {
                    livesRemaining--;
                    signalBus.Fire(new PlayerLivesAmountChangedSignal(livesRemaining));
                    if (livesRemaining == 0)
                    {
                        signalBus.Fire<PlayerDiedSignal>();
                    }
                    else
                    {
                        EnableInvulnerability().Forget();
                    }
                }
            }
        }

        private async UniTask EnableInvulnerability()
        {
            isInvulnerabilityEnabled = true;

            var renderer = GetComponentInChildren<MeshRenderer>();
            var material = renderer.material;
            renderer.material = transparentMaterial;

            await UniTask.Delay(TimeSpan.FromSeconds(playerConfiguration.InvulnerabilityDuration));
            renderer.material = material;
            isInvulnerabilityEnabled = false;
        }

        private bool CanBeDamaged()
        {
            return !isInvulnerabilityEnabled;
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<PlayerMovedSignal>(OnPlayerMoved);
            signalBus.Unsubscribe<ShotAttemptSignal>(OnShotAttempt);
        }
    }
}