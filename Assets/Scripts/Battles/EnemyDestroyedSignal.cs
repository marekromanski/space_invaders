using Battles.Entities.Enemies;

namespace Battles
{
    internal class EnemyDestroyedSignal
    {
        public EnemyType type;
        public EnemyEntity entity;

        public EnemyDestroyedSignal(EnemyType type, EnemyEntity entity)
        {
            this.type = type;
            this.entity = entity;
        }
    }
}