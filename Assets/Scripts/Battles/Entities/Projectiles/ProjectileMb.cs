using System;
using UnityEngine;

namespace Battles.Entities.Projectiles
{
    public class ProjectileMb : MonoBehaviour
    {
        private float velocity;
        private ProjectileDirection direction;

        public void SetVelocity(float velocity, ProjectileDirection direction)
        {
            this.velocity = velocity;
            this.direction = direction;
        }
    }
}