using Configs;

namespace Extensions
{
    public struct AimSettings
    {
        public float YawSpeed { get; private set; }
        public float PitchSpeed { get; private set; }
        public float MinPitch { get; private set; }
        public float MaxPitch { get; private set; }
        public float Tolerance { get; private set; }
        public ProjectileStrategyConfig StrategyConfig { get; private set; }

        public AimSettings(float yaw, float pitch, float min, float max, float tolerance, ProjectileStrategyConfig config)
        {
            YawSpeed = yaw;
            PitchSpeed = pitch;
            MinPitch = min;
            MaxPitch = max;
            Tolerance = tolerance;
            StrategyConfig = config;
        }
    }
}