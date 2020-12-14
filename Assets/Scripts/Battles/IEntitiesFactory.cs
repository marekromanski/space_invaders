using Battles.Entities;
using UnityEngine;
using Zenject;

namespace Battles
{
    public interface IEntitiesFactory
    {
        Player InstantiatePlayer(DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null);

        Enemy InstantiateEnemy(EnemyType enemyType, DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}