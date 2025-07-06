using Configs.Factory;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Strategy/Guided")]
    public class GuidedStrategyConfig : ProjectileStrategyConfig
    {
        public override T Accept<T>(IStrategyConfigVisitor<T> visitor) => visitor.Visit(this);
    }
}