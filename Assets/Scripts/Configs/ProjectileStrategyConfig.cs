using Configs.Factory;
using UnityEngine;

namespace Configs
{
    public abstract class ProjectileStrategyConfig : ScriptableObject, IProjectileStrategyConfig
    {
        [field: SerializeField]
        public AnimationCurve TrajectoryCurve { get; private set; } = AnimationCurve.Linear(0, 0, 1, 0);

        [field: SerializeField] public float ArcHeight { get; private set; } = 0f;
        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: SerializeField] public float Damage { get; private set; } = 10f;

        public abstract T Accept<T>(IStrategyConfigVisitor<T> visitor);
    }
}