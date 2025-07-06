using System.Linq;
using Configs;
using Configs.Factory;
using Enemies;
using Services;
using UnityEngine;

namespace Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [field: SerializeField] protected ShootingConfig ShootingConfig { get; private set; }
        [field: SerializeField] protected Transform ShootPoint { get; private set; }

        protected ITargetLocator TargetLocator { get; private set; }

        protected IProjectileStrategyFactory ProjectileFactory { get; private set; }

        protected float LastShotTime { get; set; }

        public void Initialize(ITargetLocator locator, IProjectileStrategyFactory projectileStrategyFactory)
        {
            TargetLocator = locator;
            ProjectileFactory = projectileStrategyFactory;
        }

        protected bool CanShoot() =>
            Time.time >= LastShotTime + ShootingConfig.ShootInterval;

        protected Monster GetNearestTarget()
        {
            var monsters = TargetLocator.GetTargetsInRange(transform.position, ShootingConfig.Range);
            return monsters
                .OrderBy(monster => Vector3.Distance(transform.position, monster.transform.position))
                .FirstOrDefault();
        }
    }
}