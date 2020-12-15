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
        private float maxShootingFrequency;

        public float MoveSpeed => moveMultiplier;
        public float ProjectileVelocity => projectileVelocity;
        public float ProjectileLifetime => projectileLifetime;
        public float MaxShootingFrequency => maxShootingFrequency;
    }
}