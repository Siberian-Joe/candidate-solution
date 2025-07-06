using Configs;
using Enemies;
using Extensions;
using Pools.Projectiles;
using UnityEngine;

namespace Towers
{
    public class CannonTower : Tower
    {
        [Header("References")] [SerializeField]
        private AimingConfig _aimingConfig;

        [SerializeField] private CannonProjectilePool _pool;
        [SerializeField] private Transform _turretPivot;
        [SerializeField] private Transform _barrelPivot;

        private Quaternion _initialTurretRotation;
        private Quaternion _initialBarrelRotation;

        private Monster _currentTarget;
        private Vector3 _lastPredictedPosition;
        private float _cachedProjectileSpeed;
        private float _cachedRangeSqr;
        private bool _isTargetValid;

        private void Awake()
        {
            _cachedProjectileSpeed = ShootingConfig.ProjectileStrategyConfig.Speed;
            _cachedRangeSqr = ShootingConfig.Range * ShootingConfig.Range;

            _initialTurretRotation = _turretPivot.rotation;
            _initialBarrelRotation = _barrelPivot.localRotation;
        }

        private void Update()
        {
            ValidateTarget();

            if (!_isTargetValid)
            {
                ResetAim();
                return;
            }

            PredictTargetPosition();
            AdjustAimWithinRange();

            if (CanShoot() && IsFullyAimed())
                Shoot();
        }

        private void ValidateTarget()
        {
            if (_currentTarget == null)
            {
                AcquireNewTarget();
                return;
            }

            _isTargetValid = transform.position.IsInRange(_currentTarget.transform.position, _cachedRangeSqr);

            if (_isTargetValid)
                return;

            ReleaseTarget();
            AcquireNewTarget();
        }

        private void AcquireNewTarget()
        {
            _currentTarget = GetNearestTarget();
            if (_currentTarget == null)
                return;

            SubscribeToTarget();
            _isTargetValid = true;
        }

        private void PredictTargetPosition()
        {
            _lastPredictedPosition = BallisticMath.CalculateInterceptPoint(
                ShootPoint.position,
                _currentTarget.transform.position,
                _currentTarget.MoveDirection * _currentTarget.Speed,
                _cachedProjectileSpeed,
                ShootingConfig.Range
            );
        }

        private void AdjustAimWithinRange()
        {
            if (!ShootPoint.position.IsBeyondMinDistance(_lastPredictedPosition, ShootingConfig.MinEngageDistance))
            {
                ResetAim();
                return;
            }

            AimHorizontally(_lastPredictedPosition);
            AimVertically(_lastPredictedPosition);
        }

        private void AimHorizontally(Vector3 targetPos)
        {
            var direction = targetPos.GetHorizontalDirection(_turretPivot.position);
            if (direction.sqrMagnitude <= 0.001f)
                return;

            var targetRotation = Quaternion.LookRotation(direction);
            _turretPivot.rotation = Quaternion.RotateTowards(
                _turretPivot.rotation,
                targetRotation,
                _aimingConfig.YawSpeed * Time.deltaTime
            );
        }

        private void AimVertically(Vector3 targetPos)
        {
            var desiredPitch = CalculateDesiredPitch(targetPos);
            var currentPitch = _barrelPivot.localEulerAngles.NormalizeAngleX();

            var newPitch = Mathf.MoveTowards(
                currentPitch,
                desiredPitch,
                _aimingConfig.PitchSpeed * Time.deltaTime
            );

            _barrelPivot.localEulerAngles = new Vector3(newPitch, 0f, 0f);
        }

        private float CalculateDesiredPitch(Vector3 targetPos)
        {
            var rawPitch = BallisticMath.CalculateRawPitchAngle(
                ShootPoint.position,
                targetPos,
                ShootingConfig.ProjectileStrategyConfig
            );

            return rawPitch
                .ClampAngle(_aimingConfig.MinPitchAngle, _aimingConfig.MaxPitchAngle)
                .InvertAngle();
        }

        private bool IsFullyAimed()
        {
            if (!ShootPoint.position.IsBeyondMinDistance(_lastPredictedPosition, ShootingConfig.MinEngageDistance))
                return false;

            return IsYawAligned() && IsPitchAligned();
        }

        private bool IsYawAligned()
        {
            var flatDirection = _lastPredictedPosition.GetFlatDirection(_turretPivot.position);
            if (flatDirection.sqrMagnitude <= 0.001f)
                return true;

            var angle = Vector3.Angle(_turretPivot.forward, flatDirection);
            return angle <= ShootingConfig.AimTolerance;
        }

        private bool IsPitchAligned()
        {
            var desiredPitch = CalculateDesiredPitch(_lastPredictedPosition);
            var currentPitch = _barrelPivot.localEulerAngles.NormalizeAngleX();
            return Mathf.Abs(currentPitch - desiredPitch) <= ShootingConfig.AimTolerance;
        }

        private void Shoot()
        {
            var projectile = _pool.Get();
            var strategy = ProjectileFactory.Create(
                ShootingConfig.ProjectileStrategyConfig,
                projectile.transform,
                ShootPoint.position,
                _lastPredictedPosition
            );

            projectile.Initialize(strategy, ShootingConfig.ProjectileStrategyConfig.Damage);
            LastShotTime = Time.time;
        }

        private void ReleaseTarget()
        {
            UnsubscribeFromTarget();
            _currentTarget = null;
            _isTargetValid = false;
        }

        private void SubscribeToTarget()
        {
            if (_currentTarget != null)
                _currentTarget.Released += HandleTargetReleased;
        }

        private void UnsubscribeFromTarget()
        {
            if (_currentTarget != null)
                _currentTarget.Released -= HandleTargetReleased;
        }

        private void HandleTargetReleased(Monster target) => ReleaseTarget();

        private void ResetAim()
        {
            // Плавный сброс горизонтального поворота
            _turretPivot.rotation = Quaternion.RotateTowards(
                _turretPivot.rotation,
                _initialTurretRotation,
                _aimingConfig.YawSpeed * Time.deltaTime
            );

            // Плавный сброс вертикального поворота
            float targetPitch = _initialBarrelRotation.eulerAngles.NormalizeAngleX();
            float currentPitch = _barrelPivot.localEulerAngles.NormalizeAngleX();

            float newPitch = Mathf.MoveTowards(
                currentPitch,
                targetPitch,
                _aimingConfig.PitchSpeed * Time.deltaTime
            );

            _barrelPivot.localEulerAngles = new Vector3(newPitch, 0f, 0f);
        }
    }
}