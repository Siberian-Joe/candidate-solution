using Enemies;
using Pools;
using Projectiles.ProjectileStrategies;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile<T> : PoolableObject<T> where T : Projectile<T>
    {
        [SerializeField] private float _maxLifetime = 5f;

        protected IProjectileStrategy ProjectileStrategy;

        private float _damage;
        private float _currentLifetime;
        private bool _isProcessed;

        private readonly Collider[] _hitBuffer = new Collider[8];
        private const float HitRadius = 0.75f;

        public virtual void Initialize(IProjectileStrategy projectileStrategy, float damage)
        {
            if (projectileStrategy == null)
            {
                Debug.LogWarning("Projectile strategy is null");
                return;
            }

            ProjectileStrategy = projectileStrategy;
            _damage = damage;
        }

        public override void ResetState()
        {
            base.ResetState();
            _currentLifetime = _maxLifetime;
            ProjectileStrategy?.ResetState();
            ProjectileStrategy = null;
            _isProcessed = false;
        }

        protected virtual void FixedUpdate()
        {
            if (ProjectileStrategy != null)
            {
                ProjectileStrategy.Move(Time.fixedDeltaTime);

                if (ProjectileStrategy.IsComplete())
                {
                    ProcessHits();
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Projectile strategy is not initialized");
                return;
            }

            _currentLifetime -= Time.fixedDeltaTime;
            if (_currentLifetime <= 0f)
                Release();
        }

        public virtual void UpdateTarget(Monster target) => ProjectileStrategy?.UpdateTarget(target);

        private void ProcessHits()
        {
            if (_isProcessed)
                return;

            _isProcessed = true;

            var count = Physics.OverlapSphereNonAlloc(
                transform.position,
                HitRadius,
                _hitBuffer
            );

            for (var i = 0; i < count; i++)
            {
                if (_hitBuffer[i].TryGetComponent<Monster>(out var monster))
                    monster.TakeDamage(_damage);
            }

            Release();
        }

        protected virtual void OnTriggerEnter(Collider other) => ProcessHits();
    }
}