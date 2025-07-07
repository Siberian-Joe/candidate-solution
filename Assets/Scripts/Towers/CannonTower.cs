using Configs;
using Enemies;
using Extensions;
using Pools.Projectiles;
using UnityEngine;

namespace Towers
{
    public class CannonTower : Tower
    {
        [Header("Aiming Components")] [SerializeField]
        private Transform _turretPivot;

        [SerializeField] private Transform _barrelPivot;

        [Header("Dependencies")] [SerializeField]
        private AimingConfig _aimingConfig;

        [SerializeField] private CannonProjectilePool _projectilePool;

        private float _projectileSpeed;
        private float _rangeSqr;
        private AimSettings _aimSettings;

        private void Awake()
        {
            _projectileSpeed = ShootingConfig.ProjectileStrategyConfig.Speed;
            _rangeSqr = ShootingConfig.Range * ShootingConfig.Range;

            _aimSettings = new AimSettings(
                _aimingConfig.YawSpeed,
                _aimingConfig.PitchSpeed,
                _aimingConfig.MinPitchAngle,
                _aimingConfig.MaxPitchAngle,
                ShootingConfig.AimTolerance,
                ShootingConfig.ProjectileStrategyConfig
            );
        }

        private void Update()
        {
            var target = TargetLocator.GetNearestInRange(transform.position, _rangeSqr);
            if (target == null)
                return;

            var interceptPoint = CalculateInterceptPoint(target);
            if (!IsInEngagementRange(interceptPoint))
                return;

            AimAtTarget(interceptPoint);

            if (CanShoot() && IsTargetLocked(interceptPoint))
                FireProjectile(interceptPoint);
        }

        private Vector3 CalculateInterceptPoint(Monster target) =>
            BallisticMath.CalculateInterceptPoint(
                ShootPoint.position,
                target.transform.position,
                target.MoveDirection * target.Speed,
                _projectileSpeed,
                ShootingConfig.Range
            );

        private bool IsInEngagementRange(Vector3 point) =>
            ShootPoint.position.IsBeyondMinDistance(point, ShootingConfig.MinEngageDistance);

        private void AimAtTarget(Vector3 targetPoint)
        {
            _turretPivot.RotateFlatTowards(targetPoint, _aimSettings);
            _barrelPivot.TiltTowards(targetPoint, _aimSettings);
        }

        private bool IsTargetLocked(Vector3 targetPoint) =>
            _turretPivot.IsFlatAligned(targetPoint, _aimSettings) &&
            _barrelPivot.IsPitchAligned(targetPoint, _aimSettings);

        private void FireProjectile(Vector3 interceptPoint)
        {
            var projectile = _projectilePool.Get();
            var strategy = ProjectileFactory.Create(
                _aimSettings.StrategyConfig,
                projectile.transform,
                ShootPoint.position,
                interceptPoint
            );

            projectile.Initialize(strategy, _aimSettings.StrategyConfig.Damage);
            LastShotTime = Time.time;
        }
    }
}