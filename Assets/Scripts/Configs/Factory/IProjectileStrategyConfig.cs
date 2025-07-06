namespace Configs.Factory
{
    public interface IProjectileStrategyConfig
    {
        T Accept<T>(IStrategyConfigVisitor<T> visitor);
    }
}