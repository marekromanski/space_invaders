using UnityEngine;
using Zenject;

namespace Battles
{
    public interface IPlayerFactory
    {
        Player InstantiatePlayer(DiContainer diContainer, Vector3 position, Quaternion rotation,
            Transform parent = null);
    }
}