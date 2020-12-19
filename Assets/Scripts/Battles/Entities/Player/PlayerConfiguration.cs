using UnityEngine;

namespace Battles.Entities.Player
{
    [CreateAssetMenu(menuName = "SpaceInvaders/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfiguration : ScriptableObject, IPlayerConfiguration
    {
        [SerializeField]
        private float moveMultiplier;

        [SerializeField]
        private float projectileVelocity;

        [SerializeField]
        private float projectileLifetime;

        [SerializeField]
        private int livesTotal;

        [SerializeField]
        private float invulnerabilityDuration = 2f;

        [SerializeField]
        private float minShootingInterval;

        public float MoveSpeed => moveMultiplier;
        public float ProjectileVelocity => projectileVelocity;
        public float ProjectileLifetime => projectileLifetime;
        public float MinShootingInterval => minShootingInterval;
        public int LivesTotal => livesTotal;
        public float InvulnerabilityDuration => invulnerabilityDuration;
    }
}