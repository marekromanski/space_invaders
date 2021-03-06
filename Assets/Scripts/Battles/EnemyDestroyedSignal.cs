﻿using Battles.Entities.Enemies;

namespace Battles
{
    internal class EnemyDestroyedSignal
    {
        public readonly EnemyType type;
        public readonly EnemyEntity entity;

        public EnemyDestroyedSignal(EnemyType type, EnemyEntity entity)
        {
            this.type = type;
            this.entity = entity;
        }
    }
}