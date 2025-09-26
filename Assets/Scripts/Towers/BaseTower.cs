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
        [SerializeField] protected float muzzleOffset = 1f;
        [SerializeField] protected TowerConfig config;

        protected Vector3 launchVelocity;

        private IProjectileLifecycleService projectileLifecycle;
        private ICooldownService shootingService;

        [Inject]
        public void Construct(IProjectileLifecycleService projectileLife)
        {
            projectileLifecycle = projectileLife;
            shootingService = new CooldownService(config.shootInterval, true);
            OnConstructed();
        }

        protected virtual void OnConstructed()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            launchVelocity = GetLaunchVelocity(deltaTime);
            if (!shootingService.IsIntervalReached(deltaTime)) return;

            projectileLifecycle.LaunchProjectile<TProjectile>(
                shootPoint.position + shootPoint.forward * muzzleOffset,
                shootPoint.forward * launchVelocity.magnitude,
                config.projectileDamage, config.gravity,
                config.aimingType is AimingType.Parabolic
            );
        }

        protected abstract Vector3 GetLaunchVelocity(float deltaTime);
    }
}