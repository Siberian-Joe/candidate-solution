using Pools;
using UnityEngine;

namespace Enemies
{
    public class Monster : PoolableObject<Monster>
    {
        [field: SerializeField] public float Speed { get; private set; } = 15f;

        [SerializeField] private float _maxHp = 30;

        private const float ReachDistance = 0.3f;

        public Vector3 MoveDirection =>
            _moveTarget == null
                ? Vector3.zero
                : (_moveTarget.position - transform.position).normalized;

        private float _currentHp;
        private Transform _moveTarget;

        public void SetMoveTarget(Transform target) => _moveTarget = target;

        public override void ResetState()
        {
            base.ResetState();
            _currentHp = _maxHp;
        }

        private void FixedUpdate()
        {
            if (_moveTarget == null)
                return;

            var direction = (_moveTarget.position - transform.position).normalized;
            transform.Translate(direction * (Speed * Time.fixedDeltaTime), Space.World);

            if (Vector3.Distance(transform.position, _moveTarget.position) <= ReachDistance)
                Release();
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= damage;

            Debug.Log($"Monster took {damage} damage. Current HP: {_currentHp}");

            if (_currentHp > 0)
                return;

            Debug.Log("Monster died");

            _currentHp = 0;
            Release();
        }
    }
}