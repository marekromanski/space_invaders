using Battles.Mechanics.LimitedLifetime;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public class ProjectileMb : MonoBehaviour, IPoolable, ITimeLimited
    {
        private float velocity;
        private ProjectileDirection direction;

        private LifetimeComponent lifetimeComponent;
        private SignalBus signalBus;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void SetCharacteristics(float velocity, ProjectileDirection direction, float lifeTime)
        {
            this.velocity = velocity;
            this.direction = direction;

            lifetimeComponent = new LifetimeComponent(lifeTime);
        }

        public void Move()
        {
            var currentPosition = transform.position;
            var normalizedDelta = direction == ProjectileDirection.Up ? 1f : -1f;
            var targetPosition = new Vector3(currentPosition.x, currentPosition.y + normalizedDelta, currentPosition.z);

            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * velocity);
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            lifetimeComponent = null;
        }

        public bool TimeElapsed()
        {
            return lifetimeComponent?.TimeElapsed() ?? false;
        }
        
        private void OnCollisionEnter()
        {
            signalBus.Fire(new ProjectileDestroyedSignal(this));
        }
    }
}