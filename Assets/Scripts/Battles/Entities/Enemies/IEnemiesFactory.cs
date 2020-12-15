using UnityEngine;
using Zenject;

namespace Battles.Entities.Enemies
{
    public interface IEnemiesFactory
    {
        EnemyEntity InstantiateEnemy(EnemyType enemyType, IInstantiator instantiator, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}