using Pools.Projectiles;
using UnityEngine;

namespace Towers
{
    public class GuidedTower : Tower
    {
        [SerializeField] private GuidedProjectilePool _pool;

        private void Update()
        {
            if (!CanShoot()) return;

            var target = GetNearestTarget();
            if (target == null) return;

            var projectile = _pool.Get();
            var strategy = ProjectileFactory.Create(
                ShootingConfig.ProjectileStrategyConfig,
                projectile.transform,
                ShootPoint.position,
                target.transform.position
            );
        
            projectile.Initialize(strategy, ShootingConfig.ProjectileStrategyConfig.Damage);
            projectile.UpdateTarget(target);

            LastShotTime = Time.time;
        }
    }
}