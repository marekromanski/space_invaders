namespace Battles.Entities.Player
{
    public interface IPlayerConfiguration
    {
        float MoveSpeed { get; }
        float ProjectileVelocity { get; }

        float MaxShootingFrequency { get; }
    }
}