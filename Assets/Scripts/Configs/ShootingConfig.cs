using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/ShootingConfig")]
    public class ShootingConfig : ScriptableObject
    {
        [field: SerializeField] public float AimTolerance { get; private set; } = 2f;
        [field: SerializeField] public float Range { get; private set; } = 10f;
        [field: SerializeField] public float ShootInterval { get; private set; } = 0.5f;
        [field: SerializeField] public float MinEngageDistance { get; private set; } = 1f;
        [field: SerializeField] public ProjectileStrategyConfig ProjectileStrategyConfig { get; private set; }
    }
}