using UnityEngine;

namespace Battles
{
    [CreateAssetMenu(menuName = "SpaceInvaders/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfiguration : ScriptableObject, IPlayerConfiguration
    {
        [SerializeField] private float moveMultiplier;

        public float MoveSpeed => moveMultiplier;
    }
}