﻿using System;
using System.Collections.Generic;
using Battles.Entities;
using Battles.Entities.Enemies;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.BattleField
{
    public class BattleOrganizer : MonoBehaviour, IWavesCounter
    {
        [SerializeField]
        private Transform playerSpawnPosition;

        private IEntitiesFactory factory;
        private DiContainer diContainer;

        private SignalBus signalBus;
        private IBattleConfig battleConfig;
        private IBattleFieldDescriptor battleFieldDescriptor;

        private readonly Dictionary<EnemyType, IEnemySpawner> enemySpawners =
            new Dictionary<EnemyType, IEnemySpawner>(3);

        private int currentWave = 0;

        [Inject, UsedImplicitly]
        private void Construct(IBattleFieldDescriptor battleFieldDescriptor,
            IBattleConfig battleConfig,
            SignalBus signalBus,
            IEntitiesFactory factory,
            DiContainer diContainer,
            IMothershipSpawner mothershipSpawner,
            IEliteEnemySpawner eliteEnemySpawner,
            IRegularEnemySpawner regularEnemySpawner)
        {
            this.battleConfig = battleConfig;
            this.signalBus = signalBus;
            this.factory = factory;
            this.diContainer = diContainer;
            this.battleFieldDescriptor = battleFieldDescriptor;

            enemySpawners.Add(EnemyType.MotherShip, mothershipSpawner);
            enemySpawners.Add(EnemyType.Elite, eliteEnemySpawner);
            enemySpawners.Add(EnemyType.Regular, regularEnemySpawner);

            signalBus.Subscribe<WaveFinishedSignal>(OnWaveFinished);
        }

        private async void OnWaveFinished()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SpawnWave();
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnWave();
        }

        private void SpawnWave()
        {
            currentWave++;
            
            int totalEnemiesRows = battleConfig.GetAmountOfRegularRows() + 2;
            float rowHeight = (battleFieldDescriptor.TopSpawnBorder - battleFieldDescriptor.BotSpawnBorder) / totalEnemiesRows;

            float mothershipRowPositionY = CalculateRowPositionY(0, rowHeight);
            float elitesRowPositionY = CalculateRowPositionY(1, rowHeight);

            SpawnRowOfEnemies(mothershipRowPositionY, EnemyType.MotherShip, 0);
            SpawnRowOfEnemies(elitesRowPositionY, EnemyType.Elite, 1);
            for (int i = 2; i < totalEnemiesRows; ++i)
            {
                float regularRowPositionY = CalculateRowPositionY(i, rowHeight);
                SpawnRowOfEnemies(regularRowPositionY, EnemyType.Regular, i);
            }
            
            signalBus.Fire(new WaveSpawnedSignal(currentWave));
        }

        private float CalculateRowPositionY(int index, float rowHeight)
        {
            return battleFieldDescriptor.TopSpawnBorder - index * rowHeight;
        }

        private void SpawnRowOfEnemies(float rowPositionY, EnemyType enemyType, int rowNumber)
        {
            var entitiesInRow = battleConfig.GetAmountOf(enemyType);
            float columnWidth = (battleFieldDescriptor.RightBorder - battleFieldDescriptor.LeftBorder) / (entitiesInRow + 1);

            for (int i = 0; i < entitiesInRow; ++i)
            {
                var x = CalculateColumnPositionX(i, columnWidth);
                SpawnEnemy(enemyType, new Vector3(x, rowPositionY, 0f), rowNumber);
            }
        }

        private float CalculateColumnPositionX(int index, float columnWidth)
        {
            return battleFieldDescriptor.LeftBorder + (index + 1) * columnWidth;
        }

        private void SpawnEnemy(EnemyType enemyType, Vector3 position, int rowNumber)
        {
            var enemy = enemySpawners[enemyType].Spawn(position, Quaternion.identity);
            enemy.Init(enemyType, rowNumber);
            signalBus.Fire(new EnemySpawnedSignal(enemyType, enemy));
        }

        private void SpawnPlayer()
        {
            factory.InstantiatePlayer(diContainer, playerSpawnPosition.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<WaveFinishedSignal>(OnWaveFinished);
        }

        public int GetAmountOfWavesBeaten()
        {
            return currentWave - 1;
        }

        public int GetCurrentWaveNumber()
        {
            return currentWave;
        }
    }

    public interface IWavesCounter
    {
        int GetAmountOfWavesBeaten();
        int GetCurrentWaveNumber();
    }
}