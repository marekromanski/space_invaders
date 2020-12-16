using UnityEngine;

namespace Battles.Entities.Enemies
{
    public interface IEnemySpawner
    {
        EnemyEntity Spawn(Vector3 position, Quaternion rotation);
    }

    public interface IMothershipSpawner : IEnemySpawner
    {
    }

    public interface IEliteEnemySpawner : IEnemySpawner
    {
    }

    public interface IRegularEnemySpawner : IEnemySpawner
    {
    }
}