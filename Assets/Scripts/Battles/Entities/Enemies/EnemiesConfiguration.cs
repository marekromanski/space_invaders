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
        private float stepSize = 1f;

        [SerializeField]
        private float stepTime = 0.5f;

        [SerializeField]
        private float waitTime = 0.5f;

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
        public float StepSize => stepSize;
        public float StepTime => stepTime;
        public float WaitTime => waitTime;

        public ICharacterConfiguration GetEnemyConfiguration(EnemyType type)
        {
            return enemies.First(x => x.type == type).configuration;
        }
    }
}