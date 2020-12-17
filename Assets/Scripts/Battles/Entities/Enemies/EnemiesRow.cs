using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities.Enemies
{
    public class EnemiesRow
    {
        private readonly EnemyType type;
        private readonly List<EnemyEntity> enemies;
        private IBattleFieldDescriptor battleFieldDescriptor;

        private const float stepSize = 1f;
        private const float stepTime = 0.5f;

        private EnemyEntity leftmostEntity;
        private EnemyEntity rightmostEntity;

        private const float WaitTime = 0.5f;

        public EnemiesRow(EnemyType enemyType, List<EnemyEntity> entities)
        {
            type = enemyType;
            enemies = entities;
            leftmostEntity = FindLeftmostEntity(enemies);
            rightmostEntity = FindRightmostEntity(enemies);
        }

        [Inject, UsedImplicitly]
        private void Construct(IBattleFieldDescriptor battleFieldDescriptor)
        {
            this.battleFieldDescriptor = battleFieldDescriptor;
            Debug.Log("Construt called");

            StartMoving().Forget();
        }

        private async UniTask StartMoving()
        {
            while (enemies.Count > 0)
            {
                await HoldPosition();
                await MoveLeft();
                await HoldPosition();
                await MoveRight();
            }
        }

        private async UniTask MoveLeft()
        {
            while (CanMoveLeft())
            {
                await TakeAStepLeft();
                await HoldPosition();
            }
        }

        private async UniTask MoveRight()
        {
            while (CanMoveRight())
            {
                await TakeAStepRight();
                await HoldPosition();
            }
        }

        private async UniTask TakeAStepLeft()
        {
            var positionAfterFullStep = leftmostEntity.transform.position.x - stepSize;
            var finalStepSize = positionAfterFullStep > battleFieldDescriptor.LeftBorder
                ? stepSize
                : Mathf.Abs(positionAfterFullStep - battleFieldDescriptor.LeftBorder);

            finalStepSize *= -1;

            await TakeAStep(finalStepSize);
        }

        private async UniTask TakeAStepRight()
        {
            var positionAfterFullStep = rightmostEntity.transform.position.x + stepSize;
            var finalStepSize = positionAfterFullStep < battleFieldDescriptor.RightBorder
                ? stepSize
                : Mathf.Abs(positionAfterFullStep - battleFieldDescriptor.RightBorder);

            await TakeAStep(finalStepSize);
        }

        private async Task TakeAStep(float finalStepSize)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (var enemy in enemies)
            {
                var tween = enemy.transform.DOMoveX(enemy.transform.position.x + finalStepSize, stepTime);
                sequence.Join(tween);
            }

            sequence.Play();
            await sequence.AsyncWaitForCompletion().AsUniTask();
        }

        private bool CanMoveLeft()
        {
            if (enemies.Count == 0)
            {
                return false;
            }

            return leftmostEntity.transform.position.x > battleFieldDescriptor.LeftBorder;
        }

        private bool CanMoveRight()
        {
            return rightmostEntity.transform.position.x < battleFieldDescriptor.RightBorder;
        }


        private async UniTask HoldPosition()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTime));
        }

        private EnemyEntity FindLeftmostEntity(List<EnemyEntity> enemyEntities)
        {
            if (enemyEntities.Count == 0)
            {
                return null;
            }

            return enemyEntities.Aggregate((curMin, x) =>
                x.transform.position.x < curMin.transform.position.x ? x : curMin);
        }

        private EnemyEntity FindRightmostEntity(List<EnemyEntity> enemyEntities)
        {
            if (enemyEntities.Count == 0)
            {
                return null;
            }

            return enemyEntities.Aggregate((curMin, x) =>
                x.transform.position.x > curMin.transform.position.x ? x : curMin);
        }

        public void Remove(EnemyEntity entity)
        {
            enemies.Remove(entity);
            if (entity == leftmostEntity)
            {
                leftmostEntity = FindLeftmostEntity(enemies);
            }

            if (entity == rightmostEntity)
            {
                rightmostEntity = FindRightmostEntity(enemies);
            }
        }

        public void Add(EnemyEntity entity)
        {
            enemies.Add(entity);
            if (leftmostEntity == null || entity.transform.position.x < leftmostEntity.transform.position.x)
            {
                leftmostEntity = entity;
            }

            if (rightmostEntity == null || entity.transform.position.x > rightmostEntity.transform.position.x)
            {
                rightmostEntity = entity;
            }
        }
    }
}