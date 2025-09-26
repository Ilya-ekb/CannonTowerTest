using Configs;
using Services.Interfaces;

namespace Services.Aiming
{
    public static class AimingFactory
    {
        public static IAimingService Create(TowerConfig config)
        {
            switch (config.aimingType)
            {
                case AimingType.Instant:
                    return new InstantAimingService();
                case AimingType.Smooth:
                    return new SmoothAimingService();
                case AimingType.Predictive:
                    return new PredictiveAimingService();
                case AimingType.Parabolic:
                    return new ParabolicAimingService();
                default:
                    return null;
            }
        }
    }
}