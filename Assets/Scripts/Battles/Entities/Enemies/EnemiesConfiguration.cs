using System;
using System.Linq;
using UnityEngine;

namespace Battles.Entities.Enemies
{
    [CreateAssetMenu(menuName = "SpaceInvaders/EnemyConfig", fileName = "EnemyConfig")]
    public class EnemiesConfiguration : ScriptableObject, IEnemiesConfiguration
    {
        [SerializeField]
        private float aimingDelta;

        [SerializeField]
        private float intervalBetweenShotAttempts;

        [SerializeField]
        private EnemyConfigurationEntry[] enemies;

        [Serializable]
        private class EnemyConfigurationEntry
        {
            public EnemyType type;
            public EnemyConfiguration configuration;
        }

        public float AimingDelta => aimingDelta;
        public float IntervalBetweenShotAttempts => intervalBetweenShotAttempts;

        public ICharacterConfiguration GetEnemyConfiguration(EnemyType type)
        {
            return enemies.First(x => x.type == type).configuration;
        }
    }
}