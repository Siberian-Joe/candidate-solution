using Enemies;

namespace Projectiles.ProjectileStrategies
{
    public interface IProjectileStrategy
    {
        void Move(float deltaTime);
        void UpdateTarget(Monster target);
        bool IsComplete();
        void ResetState();
    }
}