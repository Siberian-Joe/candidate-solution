using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static float NormalizeAngleX(this Vector3 eulerAngles)
        {
            var angle = eulerAngles.x;
            return angle > 180f ? angle - 360f : angle;
        }

        public static bool IsInRange(this Vector3 origin, Vector3 target, float rangeSqr)
        {
            return (origin - target).sqrMagnitude <= rangeSqr;
        }

        public static bool IsBeyondMinDistance(this Vector3 origin, Vector3 target, float minDistance)
        {
            var minDistanceSqr = minDistance * minDistance;
            return (origin - target).sqrMagnitude >= minDistanceSqr;
        }

        public static Vector3 GetHorizontalDirection(this Vector3 targetPos, Vector3 pivotPosition)
        {
            var horizontalTarget = new Vector3(targetPos.x, pivotPosition.y, targetPos.z);
            return horizontalTarget - pivotPosition;
        }

        public static Vector3 GetFlatDirection(this Vector3 targetPos, Vector3 origin)
        {
            var direction = targetPos - origin;
            direction.y = 0;
            return direction;
        }
    }
}