using System;
using UnityEngine;

namespace Pools
{
    public abstract class PoolableObject<T> : MonoBehaviour, IPoolable<T>
        where T : PoolableObject<T>
    {
        public event Action<T> Released;

        private Action<T> _releaseAction;
        private bool _isReleased;

        public void Initialize(Action<T> releaseAction) => _releaseAction = releaseAction;

        public virtual void ResetState() => _isReleased = false;

        public virtual void Release()
        {
            if (_isReleased)
                return;

            _isReleased = true;

            var self = (T)this;
            _releaseAction?.Invoke(self);
            Released?.Invoke(self);
        }
    }
}