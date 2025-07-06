using Configs.Factory;
using Services;
using Towers;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Tower[] _towers;

    private void Start()
    {
        var targetLocator = new TargetLocator();
        var projectileStrategyFactory = new ProjectileStrategyFactory();

        _spawner.Initialize(targetLocator);

        foreach (var tower in _towers)
            tower.Initialize(targetLocator, projectileStrategyFactory);
    }
}