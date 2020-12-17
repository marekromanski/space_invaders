using System.Collections.Generic;

namespace Battles.Entities.Enemies
{
    public class EnemiesRow
    {
        public readonly EnemyType type;
        public readonly List<EnemyEntity> enemies;
            
        public EnemyEntity MostLeftEntity { get; private set; }

        public EnemiesRow(EnemyType enemyType, List<EnemyEntity> entities)
        {
            this.type = type;
            this.enemies = entities;
        }
    }
}