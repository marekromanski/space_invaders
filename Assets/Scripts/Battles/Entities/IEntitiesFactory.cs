using Battles.Entities.Enemies;
using UnityEngine;
using Zenject;

namespace Battles.Entities
{
    public interface IEntitiesFactory
    {
        Player.PlayerMb InstantiatePlayer(DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null);

        EnemyMb InstantiateEnemy(EnemyType enemyType, DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}