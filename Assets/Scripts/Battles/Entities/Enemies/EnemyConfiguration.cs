using System;
using UnityEngine;

namespace Battles.Entities.Enemies
{
    [Serializable]
    public class EnemyConfiguration : ICharacterConfiguration
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float projectileVelocity;

        [SerializeField]
        private float projectileLifetime;

        [SerializeField]
        private float minShootingInterval;

        public float MoveSpeed => moveSpeed;
        public float ProjectileVelocity => projectileVelocity;
        public float ProjectileLifetime => projectileLifetime;
        public float MinShootingInterval => minShootingInterval;
    }
}