namespace Configs.Factory
{
    public interface IStrategyConfigVisitor<out T>
    {
        T Visit(BallisticStrategyConfig config);
        T Visit(GuidedStrategyConfig config);
    }
}