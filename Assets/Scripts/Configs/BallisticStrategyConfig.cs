using Configs.Factory;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Strategy/Ballistic")]
    public class BallisticStrategyConfig : ProjectileStrategyConfig
    {
        public override T Accept<T>(IStrategyConfigVisitor<T> visitor) => visitor.Visit(this);
    }
}