using Configs;
using Enemies;
using UnityEngine;

namespace Projectiles.ProjectileStrategies
{
    public class GuidedProjectileStrategy : ProjectileStrategy<GuidedStrategyConfig>
    {
        private readonly float _initialDistance;

        private Monster _target;

        public GuidedProjectileStrategy(GuidedStrategyConfig config,
            Transform transform,
            Vector3 start,
            Vector3 end) : base(config, transform, start, end)
        {
            _initialDistance = Vector3.Distance(start, end);
            Transform.position = start;
        }

        public override void Move(float deltaTime)
        {
            base.Move(deltaTime);

            if (_target == null)
                return;

            var distanceTravelled = Vector3.Distance(Start, Transform.position);
            var distanceProgress = Mathf.Clamp01(distanceTravelled / _initialDistance);
            var verticalOffset = Config.TrajectoryCurve.Evaluate(distanceProgress) * Config.ArcHeight;

            var targetPos = _target.transform.position;
            var direction = (targetPos - Transform.position).normalized;
            var move = direction * (Config.Speed * deltaTime);
            var vertical = Vector3.up * verticalOffset;

            Transform.position += move + vertical;

            var facing = (move + vertical).normalized;
            if (facing.sqrMagnitude > 0.001f)
            {
                Transform.rotation = Quaternion.LookRotation(facing);
            }
        }

        public override void UpdateTarget(Monster target)
        {
            base.UpdateTarget(target);

            UnsubscribeFromTarget();
            _target = target;

            if (_target != null)
                _target.Released += HandleTargetReleased;
        }

        public override bool IsComplete() => _target == null;

        public override void ResetState()
        {
            base.ResetState();

            UnsubscribeFromTarget();
            _target = null;
        }

        private void HandleTargetReleased(Monster releasedTarget)
        {
            UnsubscribeFromTarget();
            _target = null;
        }

        private void UnsubscribeFromTarget()
        {
            if (_target != null)
                _target.Released -= HandleTargetReleased;
        }
    }
}