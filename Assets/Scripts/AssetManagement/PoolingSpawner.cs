using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AssetManagement
{
    public abstract class PoolingSpawner<T> : IDisposable where T : MonoBehaviour, IPoolable
    {
        protected readonly Stack<T> objectPool = new Stack<T>();

        public T Spawn(Vector3 position, Quaternion rotation)
        {
            if (objectPool.Count > 0)
            {
                var projectile = objectPool.Pop();
                projectile.transform.SetPositionAndRotation(position, rotation);
                projectile.OnSpawned();
                return projectile;
            }

            return SpawnNewInstance(position, rotation);
        }

        protected abstract T SpawnNewInstance(Vector3 position, Quaternion rotation);

        protected abstract void SubscribeToDestroySignal();

        protected abstract void UnsubscribeToDestroySignal();

        public void Dispose()
        {
            UnsubscribeToDestroySignal();
        }
    }
}