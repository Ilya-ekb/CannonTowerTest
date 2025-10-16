using Projectiles;
using Services.Aiming;
using Services.Interfaces;
using UnityEngine;

namespace Towers
{
    public class CannonTower : BaseTower<CannonProjectile>
    {
        [SerializeField] private float minAimToleranceIdDegrees = 1.5f;
        [SerializeField] private float maxAimToleranceIdDegrees = 5f;
        [SerializeField] private Turret turret;

        private IAimingService aimingService;

        protected override void OnConstructed()
        {
            base.OnConstructed();
            aimingService = AimingFactory.Create(config);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            turret.ApplyLaunchVelocityToTurret(launchVelocity, config.rotateSpeed, deltaTime);
        }

        protected override Vector3 GetLaunchVelocity(float deltaTime)
        {
            if (base.CanFire())
                return aimingService.GetAimDirection(shootPoint,
                    target,
                    config.projectileSpeed,
                    config.gravity);

            return shootPoint.forward * config.projectileSpeed;
        }

        protected override bool CanFire()
        {
            if (target is null) return false;
            
            // Normalized direction where we want to aim (from aiming service)
            Vector3 desiredDir = launchVelocity.normalized;
            Vector3 currentDir = shootPoint.forward;

            // Calculate angular difference in degrees
            float angleError = Vector3.Angle(currentDir, desiredDir);

            // Define tolerance (e.g., 2°–5° depending on rotation speed and projectile speed)
            float allowedError = Mathf.Lerp(minAimToleranceIdDegrees, maxAimToleranceIdDegrees,
                target.Velocity.magnitude / config.projectileSpeed);

            bool aligned = angleError <= allowedError;
            bool inRange = Vector3.Distance(target.Position, shootPoint.position) <= config.findTargetDistance;

            return aligned && inRange;
        }
    }
}