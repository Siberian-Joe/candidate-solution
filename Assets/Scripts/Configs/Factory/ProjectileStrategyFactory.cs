using Projectiles.ProjectileStrategies;
using UnityEngine;

namespace Configs.Factory
{
    public class ProjectileStrategyFactory : IProjectileStrategyFactory
    {
        private readonly StrategyCreationVisitor _visitor = new();

        public IProjectileStrategy Create(
            ProjectileStrategyConfig config,
            Transform projectileTransform,
            Vector3 start,
            Vector3 end) =>
            config.Accept(_visitor.Create(projectileTransform, start, end));

        private class StrategyCreationVisitor : IStrategyConfigVisitor<IProjectileStrategy>
        {
            private Transform _transform;
            private Vector3 _start;
            private Vector3 _end;

            public IStrategyConfigVisitor<IProjectileStrategy> Create(
                Transform transform,
                Vector3 start,
                Vector3 end)
            {
                _transform = transform;
                _start = start;
                _end = end;
                return this;
            }

            public IProjectileStrategy Visit(BallisticStrategyConfig config) =>
                new BallisticProjectileStrategy(config, _transform, _start, _end);

            public IProjectileStrategy Visit(GuidedStrategyConfig config) =>
                new GuidedProjectileStrategy(config, _transform, _start, _end);
        }
    }
}