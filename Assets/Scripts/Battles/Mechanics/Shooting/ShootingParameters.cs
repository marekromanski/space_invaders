using Battles.Entities.Projectiles;
using UnityEngine;

namespace Battles.Mechanics.Shooting
{
    public class ShootingParameters
    {
        public float projectileVelocity;
        public float minShotsInterval;
        public ProjectileDirection direction;
        public Transform spawn;
        public float lifeTime;

        public ShootingParameters(float projectileVelocity, float minShotsInterval, Transform spawn, ProjectileDirection direction, float lifeTime)
        {
            this.projectileVelocity = projectileVelocity;
            this.minShotsInterval = minShotsInterval;
            this.spawn = spawn;
            this.direction = direction;
            this.lifeTime = lifeTime;
        }
    }
}