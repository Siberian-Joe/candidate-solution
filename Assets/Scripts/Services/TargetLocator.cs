using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Services
{
    public class TargetLocator : ITargetLocator
    {
        private readonly List<Monster> _monsters = new();

        public void Register(Monster monster)
        {
            _monsters.Add(monster);
            monster.Released += HandleMonsterReleased;
        }

        private void HandleMonsterReleased(Monster monster)
        {
            monster.Released -= HandleMonsterReleased;
            _monsters.Remove(monster);
        }

        public IReadOnlyList<Monster> GetTargetsInRange(Vector3 point, float range)
        {
            var rangeSqr = range * range;
            return _monsters
                .Where(monster => (monster.transform.position - point).sqrMagnitude <= rangeSqr)
                .ToList();
        }
    }
}