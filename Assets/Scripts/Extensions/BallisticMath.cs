using Configs;
using UnityEngine;

namespace Extensions
{
    public static class BallisticMath
    {
        public static Vector3 CalculateInterceptPoint(
            Vector3 origin, 
            Vector3 targetPos, 
            Vector3 targetVelocity, 
            float projectileSpeed, 
            float maxRange)
        {
            if (projectileSpeed <= Mathf.Epsilon) return targetPos;

            var predicted = targetPos;
            var maxTime = maxRange / projectileSpeed;

            for (var i = 0; i < 3; i++)
            {
                var toTarget = predicted - origin;
                var time = toTarget.magnitude / projectileSpeed;
                time = Mathf.Clamp(time, 0f, maxTime);
                predicted = targetPos + targetVelocity * time;
            }

            return predicted;
        }

        public static float CalculateRawPitchAngle(
            Vector3 origin, 
            Vector3 targetPos, 
            ProjectileStrategyConfig cfg)
        {
            var delta = targetPos - origin;
            var horizontalDistance = new Vector2(delta.x, delta.z).magnitude;
            
            if (horizontalDistance < 0.001f)
            {
                return delta.y > 0 ? 90f : -90f;
            }

            const float tNorm = 0.1f;

            var h0 = cfg.TrajectoryCurve.Evaluate(0f) * cfg.ArcHeight;
            var h1 = cfg.TrajectoryCurve.Evaluate(tNorm) * cfg.ArcHeight;
            var dy = h1 - h0;
            var dx = horizontalDistance * tNorm;

            var angleToTarget = Mathf.Atan2(delta.y, horizontalDistance);
            var trajectorySlope = Mathf.Atan2(dy, dx);
            
            return (angleToTarget + trajectorySlope) * Mathf.Rad2Deg;
        }

        public static float ClampAngle(this float angle, float min, float max)
        {
            return Mathf.Clamp(angle, min, max);
        }

        public static float InvertAngle(this float angle)
        {
            return -angle;
        }
    }
}