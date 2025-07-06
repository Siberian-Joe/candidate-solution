using System.Collections;
using Enemies;
using Pools;
using UnityEngine;

namespace Services
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float _interval = 3f;
        [SerializeField] private Transform _moveTarget;
        [SerializeField] private GameObjectPool<Monster> _monsterPool;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private bool _spawning;

        private ITargetLocator _targetLocator;
        private Coroutine _spawnRoutine;

        public void Initialize(ITargetLocator targetLocator)
        {
            _targetLocator = targetLocator;

            StartSpawning();
        }

        [ContextMenu("Start Spawning")]
        public void StartSpawning()
        {
            if (_spawning)
                return;

            _spawning = true;
            _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning() => _spawning = false;

        private IEnumerator SpawnLoop()
        {
            var wait = new WaitForSeconds(_interval);
            while (_spawning)
            {
                yield return wait;

                if (!_spawning)
                    yield break;

                var monster = _monsterPool.Get();
                monster.transform.position = _spawnPoint.position;
                monster.SetMoveTarget(_moveTarget);
                _targetLocator.Register(monster);
            }
        }

        private void OnDisable()
        {
            StopSpawning();

            if (_spawnRoutine == null)
                return;

            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
}