namespace Battles.Entities.Projectiles
{
    internal class ProjectileDestroyedSignal
    {
        public readonly ProjectileMb projectile;

        public ProjectileDestroyedSignal(ProjectileMb projectile)
        {
            this.projectile = projectile;
        }
    }
}