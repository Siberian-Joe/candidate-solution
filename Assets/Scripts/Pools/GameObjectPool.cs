using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class GameObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable<T>
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _defaultCapacity = 10;
    
        private ObjectPool<T> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<T>(
                createFunc: CreatePooledItem,
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnedToPool,
                actionOnDestroy: OnDestroyPoolObject,
                defaultCapacity: _defaultCapacity
            );
        }

        public T Get() => _pool.Get();
        public void Release(T item) => _pool.Release(item);

        private T CreatePooledItem()
        {
            var item = Instantiate(_prefab, _container);
            item.gameObject.SetActive(false);
            item.Initialize(_pool.Release);
            return item;
        }

        private static void OnTakeFromPool(T item)
        {
            item.ResetState();
            item.gameObject.SetActive(true);
        }

        private static void OnReturnedToPool(T item) => item.gameObject.SetActive(false);

        private static void OnDestroyPoolObject(T item) => Destroy(item.gameObject);
    }
}