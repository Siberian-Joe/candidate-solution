using Configs;
using UnityEngine;

namespace Projectiles.ProjectileStrategies
{
    public class BallisticProjectileStrategy : ProjectileStrategy<BallisticStrategyConfig>
    {
        private readonly float _flightTime;

        private float _elapsed;

        public BallisticProjectileStrategy(BallisticStrategyConfig config,
            Transform transform,
            Vector3 start,
            Vector3 end) : base(config, transform, start, end)
        {
            var distance = Vector3.Distance(Start, End);
            _flightTime = distance / Mathf.Max(Config.Speed, 0.01f);
        }

        public override void Move(float deltaTime)
        {
            base.Move(deltaTime);

            _elapsed += deltaTime;
            var t = Mathf.Clamp01(_elapsed / _flightTime);

            var flatPos = Vector3.Lerp(Start, End, t);
            var height = Config.TrajectoryCurve.Evaluate(t) * Config.ArcHeight;
            Transform.position = flatPos + Vector3.up * height;

            UpdateRotation(t);
        }

        public override bool IsComplete() => _elapsed >= _flightTime;

        private void UpdateRotation(float t)
        {
            var nextT = Mathf.Clamp01(t + 0.01f);
            var currentPos = GetPosition(t);
            var nextPos = GetPosition(nextT);

            var direction = nextPos - currentPos;
            if (direction.sqrMagnitude > 0.001f)
                Transform.rotation = Quaternion.LookRotation(direction.normalized);
        }

        private Vector3 GetPosition(float t)
        {
            var flatPos = Vector3.Lerp(Start, End, t);
            var height = Config.TrajectoryCurve.Evaluate(t) * Config.ArcHeight;
            return flatPos + Vector3.up * height;
        }
    }
}