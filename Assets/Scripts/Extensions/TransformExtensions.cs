using Configs;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void RotateFlatTowards(this Transform turret, Vector3 target, AimSettings aim)
        {
            var direction = target - turret.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.001f) return;
            var to = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.RotateTowards(
                turret.rotation,
                to,
                aim.YawSpeed * Time.deltaTime);
        }

        public static void TiltTowards(this Transform barrel, Vector3 target, AimSettings aim)
        {
            var desired = CalculatePitch(barrel, target, aim.StrategyConfig, aim.MinPitch,
                aim.MaxPitch);
            var current = barrel.localEulerAngles.x.NormalizeAngle();
            var next = Mathf.MoveTowards(current, desired, aim.PitchSpeed * Time.deltaTime);
            barrel.localEulerAngles = new Vector3(next, 0f, 0f);
        }

        public static bool IsFlatAligned(this Transform turret, Vector3 target, AimSettings aim)
        {
            var f = turret.forward;
            f.y = 0f;
            var to = target - turret.position;
            to.y = 0f;
            if (to.sqrMagnitude <= 0.001f) return true;
            return Vector3.Angle(f, to.normalized) <= aim.Tolerance;
        }

        public static bool IsPitchAligned(this Transform barrel, Vector3 target, AimSettings aim)
        {
            var desired = CalculatePitch(barrel, target, aim.StrategyConfig, aim.MinPitch,
                aim.MaxPitch);
            var current = barrel.localEulerAngles.x.NormalizeAngle();
            return Mathf.Abs(current - desired) <= aim.Tolerance;
        }

        public static float CalculatePitch(this Transform barrel, Vector3 target, ProjectileStrategyConfig cfg,
            float minAngle, float maxAngle)
        {
            var origin = barrel.position;
            var delta = target - origin;
            var h = new Vector2(delta.x, delta.z).magnitude;
            var los = h > 0f ? Mathf.Atan2(delta.y, h) : Mathf.Sign(delta.y) * Mathf.PI / 2f;
            const float sample = 0.25f;
            var a0 = cfg.TrajectoryCurve.Evaluate(0f) * cfg.ArcHeight;
            var a1 = cfg.TrajectoryCurve.Evaluate(sample) * cfg.ArcHeight;
            var slope = h * sample > 0f ? (a1 - a0) / (h * sample) : 0f;
            var curve = Mathf.Atan(slope);
            var angle = (los + curve) * Mathf.Rad2Deg;
            var clamped = Mathf.Clamp(angle, minAngle, maxAngle);
            return -clamped;
        }
    }
}