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
            if (projectileSpeed <= Mathf.Epsilon)
                return targetPos;

            var predicted = targetPos;
            var maxTime = maxRange / projectileSpeed;
            for (var i = 0; i < 3; i++)
            {
                var toPredicted = predicted - origin;
                var t = Mathf.Clamp(toPredicted.magnitude / projectileSpeed, 0f, maxTime);
                predicted = targetPos + targetVelocity * t;
            }

            return predicted;
        }
    }
}