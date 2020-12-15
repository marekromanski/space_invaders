using UnityEngine;

namespace Battles.Entities.Projectiles
{
    public class SpawnProjectileSignal
    {
        public Vector3 position;
        public float velocity;
        public ProjectileDirection direction;
        public float lifeTime;

        public SpawnProjectileSignal(Vector3 position, float velocity, ProjectileDirection direction, float lifeTime)
        {
            this.position = position;
            this.velocity = velocity;
            this.direction = direction;
            this.lifeTime = lifeTime;
        }
    }
}