using Projectiles.ProjectileStrategies;
using UnityEngine;

namespace Configs.Factory
{
    public interface IProjectileStrategyFactory
    {
        IProjectileStrategy Create(
            ProjectileStrategyConfig config,
            Transform projectileTransform,
            Vector3 start,
            Vector3 end);
    }
}