using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Services
{
    public interface ITargetLocator
    {
        public void Register(Monster monster);
        IReadOnlyList<Monster> GetTargetsInRange(Vector3 center, float range);
    }
}