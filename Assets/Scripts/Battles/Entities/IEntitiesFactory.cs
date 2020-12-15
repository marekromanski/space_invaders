using Battles.Entities.Enemies;
using Battles.Entities.Player;
using Battles.Entities.Projectiles;

namespace Battles.Entities
{
    public interface IEntitiesFactory : IPlayerFactory, IEnemiesFactory, IProjectileFactory
    {
    }
}