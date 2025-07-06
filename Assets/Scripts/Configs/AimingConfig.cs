using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/AimingConfig")]
    public class AimingConfig : ScriptableObject
    {
        [field: SerializeField] public float YawSpeed { get; private set; } = 180f;
        [field: SerializeField] public float PitchSpeed { get; private set; } = 90f;
        [field: SerializeField] public float MinPitchAngle { get; private set; } = -5f;
        [field: SerializeField] public float MaxPitchAngle { get; private set; } = 45f;
    }
}