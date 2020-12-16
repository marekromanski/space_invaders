using UnityEngine;

namespace Battles.Entities.Projectiles
{
    public interface IProjectileSpawner
    {
        ProjectileMb Spawn(Vector3 position, Quaternion rotation);
    }
}