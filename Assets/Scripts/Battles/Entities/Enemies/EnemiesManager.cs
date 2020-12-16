using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemiesManager : IDisposable, ITickable
    {
        private readonly SignalBus signalBus;

        private RaycastHit[] reycastHits;
        private float lastShotTakenTime;
        private float minShootingInterval = 0.5f;


        private readonly Dictionary<EnemyType, List<EnemyEntity>> activeEnemies =
            new Dictionary<EnemyType, List<EnemyEntity>>
            {
                {EnemyType.MotherShip, new List<EnemyEntity>()},
                {EnemyType.Elite, new List<EnemyEntity>()},
                {EnemyType.Regular, new List<EnemyEntity>()},
            };


        [UsedImplicitly]
        public EnemiesManager(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            signalBus.Subscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

            reycastHits = new RaycastHit[10];
            lastShotTakenTime = Time.time;
        }

        private void OnEnemySpawned(EnemySpawnedSignal signal)
        {
            activeEnemies[signal.type].Add(signal.entity);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            activeEnemies[signal.type].Remove(signal.entity);
        }

        public void Tick()
        {
            if (lastShotTakenTime + minShootingInterval > Time.time)
            {
                return;
            }
            
            var allEnemies = GetAllEnemiesAsList();

            foreach (var enemy in allEnemies)
            {
                if (HasValidTarget(enemy))
                {
                    Debug.Log("I am shooting", enemy.gameObject);
                    enemy.AttemptShot();
                    lastShotTakenTime = Time.time;
                    break;
                }
            }
        }

        private List<EnemyEntity> GetAllEnemiesAsList()
        {
            var allEnemies = new List<EnemyEntity>();
            foreach (var listOfEnmies in activeEnemies.Values)
            {
                allEnemies.AddRange(listOfEnmies);
            }

            return allEnemies;
        }

        private bool HasValidTarget(EnemyEntity enemy)
        {
            var aimDelta = 0.5f;
            var aimLeft = new Vector3(enemy.ProjectileSpawnPosition.x - aimDelta, enemy.ProjectileSpawnPosition.y, enemy.ProjectileSpawnPosition.z);
            var aimRight = new Vector3(enemy.ProjectileSpawnPosition.x + aimDelta, enemy.ProjectileSpawnPosition.y, enemy.ProjectileSpawnPosition.z);
            var aimLeftResult = Aim(aimLeft);
            var aimRightResult = Aim(aimRight);

            return !(aimLeftResult.enemyInFront || aimRightResult.enemyInFront ) && (aimLeftResult.playerInFront || aimLeftResult.playerInFront);
        }

        private AimingResult Aim(Vector3 raycastOrigin)
        {
            int hits = Physics.RaycastNonAlloc(raycastOrigin, Vector3.down, reycastHits, Mathf.Infinity);

            var result = new AimingResult();
            for (int i = 0; i < hits; ++i)
            {
                if (reycastHits[i].collider.CompareTag(Tags.ENEMY))
                {
                    result.enemyInFront = true;
                }
            }

            for (int i = 0; i < hits; ++i)
            {
                if (reycastHits[i].collider.CompareTag(Tags.PLAYER))
                {
                    result.playerInFront = true;
                }
            }

            return result;
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        private class AimingResult
        {
            public bool enemyInFront;
            public bool playerInFront;
        }
    }
}