using UnityEngine;
using Zenject;

namespace Battles.Entities.Projectiles
{
    public interface IProjectileFactory
    {
        ProjectileMb InstantiateProjectile(IInstantiator instantiator, Vector3 position,
            Quaternion rotation, Transform parent = null);
    }
}