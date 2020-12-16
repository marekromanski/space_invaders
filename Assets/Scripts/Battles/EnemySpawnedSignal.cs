using Battles.Entities.Enemies;

namespace Battles
{
    public class EnemySpawnedSignal
    {
        public readonly EnemyType type;
        public readonly EnemyEntity entity;

        public EnemySpawnedSignal(EnemyType type, EnemyEntity entity)
        {
            this.type = type;
            this.entity = entity;
        }
    }
}