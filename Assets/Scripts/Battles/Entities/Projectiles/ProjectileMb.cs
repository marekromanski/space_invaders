using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public class ProjectileMb : MonoBehaviour, IPoolable
    {
        private float velocity;
        private ProjectileDirection direction;

        public void SetVelocity(float velocity, ProjectileDirection direction)
        {
            this.velocity = velocity;
            this.direction = direction;
        }

        public void Move()
        {
            var currentPosition = transform.position;
            var normalizedDelta = direction == ProjectileDirection.Up ? 1f : -1f;
            var targetPosition = new Vector3(currentPosition.x, currentPosition.y + normalizedDelta, currentPosition.z);

            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * velocity);
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