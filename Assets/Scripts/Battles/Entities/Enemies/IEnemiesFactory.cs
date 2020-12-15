using UnityEngine;
using Zenject;

namespace Battles.Entities.Enemies
{
    public interface IEnemiesFactory
    {
        EnemyMb InstantiateEnemy(EnemyType enemyType, IInstantiator instantiator, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}