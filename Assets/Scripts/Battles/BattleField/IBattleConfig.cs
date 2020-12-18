using Battles.Entities.Enemies;

namespace Battles.BattleField
{
    public interface IBattleConfig
    {
        int GetAmountOf(EnemyType type);

        int GetAmountOfRegularRows();
    }
}