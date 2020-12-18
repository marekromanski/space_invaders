using System;
using Battles.Entities.Enemies;
using UnityEngine;

namespace Battles.BattleField
{
    [CreateAssetMenu(menuName = "SpaceInvaders/BattleConfig", fileName = "BattleConfig")]
    public class BattleConfig : ScriptableObject, IBattleConfig
    {
        [SerializeField]
        private int amountOfMotherhips;

        [SerializeField]
        private int amountOfElites;

        [SerializeField]
        private int amountOfRegulars;

        [SerializeField]
        private int amountOfRegularRows;

        public int GetAmountOfRegularRows() => amountOfRegularRows;

        public int GetAmountOf(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.MotherShip:
                    return amountOfMotherhips;
                case EnemyType.Elite:
                    return amountOfElites;
                case EnemyType.Regular:
                    return amountOfRegulars;
                default:
                    throw new ArgumentException($"Unexpected value for EnemyType: {type}");
            }
        }

    }
}