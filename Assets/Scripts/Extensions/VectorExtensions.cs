using System.Linq;
using Enemies;
using Services;
using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static float NormalizeAngle(this float angle)
            => angle > 180f ? angle - 360f : angle;

        public static bool IsBeyondMinDistance(this Vector3 origin, Vector3 target, float minDistance)
            => (origin - target).sqrMagnitude >= minDistance * minDistance;

        public static Monster GetNearestInRange(this ITargetLocator locator, Vector3 position, float rangeSqr)
            => locator.GetTargetsInRange(position, Mathf.Sqrt(rangeSqr))
                .OrderBy(monster => (monster.transform.position - position).sqrMagnitude)
                .FirstOrDefault();
    }
}