using Configs;
using Core;
using Services;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Towers
{
    public abstract class BaseTower<TProjectile> : UpdateableBehaviour where TProjectile : class, IProjectile
    {
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected TowerConfig config;

        protected Vector3 launchVelocity;

        private IProjectileLifecycleService projectileLifecycle;
        private ITargetService targetService;
        private ICooldownService reloadCooldown;

        protected IShootTarget target;
        private bool isReloaded = false;
        private bool useGravity = false;

        [Inject]
        public void Construct(IProjectileLifecycleService projectileLife, ITargetService targetServ)
        {
            projectileLifecycle = projectileLife;
            targetService = targetServ;
            reloadCooldown = new CooldownService(config.shootInterval, true);
            useGravity = config.aimingType is AimingType.Parabolic;
            OnConstructed();
        }

        protected virtual void OnConstructed()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            var shootPosition = shootPoint.position;
            target = targetService.GetTarget(shootPosition, config.findTargetDistance);
            launchVelocity = GetLaunchVelocity(deltaTime);

            if (!isReloaded)
                isReloaded = reloadCooldown.IsIntervalReached(deltaTime);
            if (!isReloaded) return;

            if (!CanFire()) return;
            var shootVelocity = shootPoint.forward * config.projectileSpeed;
            var grav = useGravity ? config.gravity : Vector3.zero;
            var damage = config.projectileDamage;
            projectileLifecycle.LaunchProjectile<TProjectile>(shootPosition, shootVelocity, damage, grav, useGravity);
            reloadCooldown.SetInterval(config.shootInterval);
            isReloaded = reloadCooldown.IsIntervalReached(deltaTime);
        }

        protected abstract Vector3 GetLaunchVelocity(float deltaTime);

        protected virtual bool CanFire()
        {
            return target is not null;
        }
    }
}