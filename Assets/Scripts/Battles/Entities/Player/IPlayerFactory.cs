using UnityEngine;
using Zenject;

namespace Battles.Entities.Player
{
    public interface IPlayerFactory
    {
        PlayerEntity InstantiatePlayer(IInstantiator instantiator, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}