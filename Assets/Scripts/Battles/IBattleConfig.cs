using Battles.Entities.Enemies;

namespace Battles
{
    public interface IBattleConfig
    {
        int GetAmountOf(EnemyType type);

        int GetAmountOfRegularRows();
    }
}