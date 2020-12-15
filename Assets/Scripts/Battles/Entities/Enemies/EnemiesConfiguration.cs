using System;
using System.Linq;
using UnityEngine;

namespace Battles.Entities.Enemies
{
    [CreateAssetMenu(menuName = "SpaceInvaders/EnemyConfig", fileName = "EnemyConfig")]
    public class EnemiesConfiguration : ScriptableObject, IEnemiesConfiguration
    {
        [SerializeField]
        private EnemyConfigurationEntry[] enemies;
        
        [Serializable]
        private class EnemyConfigurationEntry
        {
            public EnemyType type;
            public EnemyConfiguration configuration;
        }

        public ICharacterConfiguration GetEnemyConfiguration(EnemyType type)
        {
            return enemies.First(x => x.type == type).configuration;
        }
    }
}