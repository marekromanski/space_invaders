using System;
using Battles.Entities.Enemies;
using UnityEngine;

namespace Battles.Scoring
{
    [CreateAssetMenu(menuName = "SpaceInvaders/ScoringConfig", fileName = "ScoringConfig")]

    public class ScoringConfiguration : ScriptableObject, IScoringConfiguration
    {
        [SerializeField]
        private int scoreForMotherShip;
        
        [SerializeField]
        private int scoreForElite;
        
        [SerializeField]
        private int scoreForRegular;
        
        public int GetPointsFor(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.MotherShip:
                    return scoreForMotherShip;
                case EnemyType.Elite:
                    return scoreForElite;
                case EnemyType.Regular:
                    return scoreForRegular;
                default:
                    throw new ArgumentException($"Unexpected value for enemyType: {enemyType}");
            }
        }
    }
}