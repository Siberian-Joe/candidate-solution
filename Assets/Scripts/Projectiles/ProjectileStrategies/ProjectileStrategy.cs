using Configs;
using Enemies;
using UnityEngine;

namespace Projectiles.ProjectileStrategies
{
    public abstract class ProjectileStrategy<TConfig> : IProjectileStrategy where TConfig : ProjectileStrategyConfig
    {
        protected TConfig Config { get; private set; }
        protected Transform Transform { get; private set; }
        protected Vector3 Start { get; private set; }
        protected Vector3 End { get; private set; }

        protected ProjectileStrategy(TConfig config, Transform transform, Vector3 start, Vector3 end)
        {
            Config = config;
            Transform = transform;
            Start = start;
            End = end;
        }

        public virtual void Move(float deltaTime)
        {
        }

        public virtual void UpdateTarget(Monster target)
        {
        }

        public virtual void ResetState() => Transform.position = Start;

        public abstract bool IsComplete();
    }
}