namespace Battles.Entities
{
    public interface ICharacterConfiguration
    {
        float MoveSpeed { get; }
        float ProjectileVelocity { get; }
        float ProjectileLifetime { get; }

        float MinShootingInterval { get; }
    }
}