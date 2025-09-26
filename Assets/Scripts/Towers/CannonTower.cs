using Projectiles;
using Services.Aiming;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Towers
{
    public class CannonTower : BaseTower<CannonProjectile>
    {
        [SerializeField] private Turret turret;

        private ITargetService targetService;
        private IAimingService aimingService;

        [Inject]
        public void ConstructCannon(ITargetService targetServ)
        {
            targetService = targetServ;
            aimingService = AimingFactory.Create(config);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            turret.ApplyLaunchVelocityToTurret(launchVelocity, config.rotateSpeed, deltaTime);
        }

        protected override Vector3 GetLaunchVelocity(float deltaTime)
        {
            var target = targetService.GetTarget(shootPoint.position, config.findTargetDistance);

            if (target is not null)
                return aimingService.GetAimDirection(shootPoint,
                    target,
                    config.projectileSpeed,
                    config.gravity);

            return shootPoint.forward * config.projectileSpeed;
        }
    }
}