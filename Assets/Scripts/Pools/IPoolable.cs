using System;
using UnityEngine;

namespace Pools
{
    public interface IPoolable<out T> where T : MonoBehaviour
    {
        void Initialize(Action<T> releaseAction);
        void ResetState();
        void Release();
    }
}