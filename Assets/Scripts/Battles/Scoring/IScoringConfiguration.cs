using Battles.Entities.Enemies;

namespace Battles.Scoring
{
    public interface IScoringConfiguration
    {
        int GetPointsFor(EnemyType enemyType);
    }
}