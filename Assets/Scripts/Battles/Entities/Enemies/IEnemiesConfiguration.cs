namespace Battles.Entities.Enemies
{
    public interface IEnemiesConfiguration
    {
        ICharacterConfiguration GetEnemyConfiguration(EnemyType type);
    }
}