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
        private readonly IEnemiesConfiguration enemiesConfiguration;

        private readonly RaycastHit[] reycastHits = new RaycastHit[10];

        private float lastShotTakenTime;

        private readonly Dictionary<EnemyType, List<EnemyEntity>> activeEnemiesByType =
            new Dictionary<EnemyType, List<EnemyEntity>>
            {
                {EnemyType.MotherShip, new List<EnemyEntity>()},
                {EnemyType.Elite, new List<EnemyEntity>()},
                {EnemyType.Regular, new List<EnemyEntity>()},
            };

        private readonly Dictionary<int, EnemiesRow> enemiesByRows = new Dictionary<int, EnemiesRow>();
        private readonly DiContainer diContainer;

        [UsedImplicitly]
        public EnemiesManager(SignalBus signalBus, IEnemiesConfiguration enemiesConfiguration, DiContainer diContainer)
        {
            this.signalBus = signalBus;
            this.enemiesConfiguration = enemiesConfiguration;
            this.diContainer = diContainer;

            signalBus.Subscribe<EnemySpawnedSignal>(OnEnemySpawned);
            signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

            lastShotTakenTime = Time.time;
        }

        private void OnEnemySpawned(EnemySpawnedSignal signal)
        {
            activeEnemiesByType[signal.type].Add(signal.entity);
            if (enemiesByRows.ContainsKey(signal.entity.RowNumber))
            {
                enemiesByRows[signal.entity.RowNumber].Add(signal.entity);
            }
            else
            {
                var enemiesRow = new EnemiesRow(signal.type, new List<EnemyEntity> {signal.entity});
                diContainer.Inject(enemiesRow);
                enemiesByRows.Add(signal.entity.RowNumber, enemiesRow);
            }
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            activeEnemiesByType[signal.type].Remove(signal.entity);
            enemiesByRows[signal.entity.RowNumber].Remove(signal.entity);
        }

        public void Tick()
        {
            AttemptShot();
        }

        private void AttemptShot()
        {
            if (lastShotTakenTime + enemiesConfiguration.IntervalBetweenShotAttempts > Time.time)
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
            foreach (var listOfEnmies in activeEnemiesByType.Values)
            {
                allEnemies.AddRange(listOfEnmies);
            }

            return allEnemies;
        }

        private bool HasValidTarget(EnemyEntity enemy)
        {
            var aimLeft = new Vector3(enemy.ProjectileSpawnPosition.x - enemiesConfiguration.AimingDelta,
                enemy.ProjectileSpawnPosition.y, enemy.ProjectileSpawnPosition.z);
            var aimRight = new Vector3(enemy.ProjectileSpawnPosition.x + enemiesConfiguration.AimingDelta,
                enemy.ProjectileSpawnPosition.y, enemy.ProjectileSpawnPosition.z);
            var aimLeftResult = Aim(aimLeft);
            var aimRightResult = Aim(aimRight);

            return !(aimLeftResult.enemyInFront || aimRightResult.enemyInFront) &&
                   (aimLeftResult.playerInFront || aimRightResult.playerInFront);
        }

        private AimingResult Aim(Vector3 raycastOrigin)
        {
            int hits = Physics.RaycastNonAlloc(raycastOrigin, Vector3.down, reycastHits, Mathf.Infinity);

            return new AimingResult
            {
                enemyInFront = IsEntityWithTagInFront(hits, Tags.ENEMY),
                playerInFront = IsEntityWithTagInFront(hits, Tags.PLAYER)
            };
        }

        private bool IsEntityWithTagInFront(int hits, string tag)
        {
            for (int i = 0; i < hits; ++i)
            {
                if (reycastHits[i].collider.CompareTag(tag))
                {
                    return true;
                }
            }

            return false;
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