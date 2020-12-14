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

        public float MoveSpeed => moveMultiplier;
        public float ProjectileVelocity => projectileVelocity;
    }
}