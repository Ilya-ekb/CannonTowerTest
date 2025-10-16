using Core;
using UnityEngine;
using Services.Interfaces;

namespace Services
{
    public class ProjectileLifecycleService : IProjectileLifecycleService
    {
        private readonly IPoolService poolService;
        private readonly IHitService hitService;

        public ProjectileLifecycleService(IPoolService poolService, IHitService hitService)
        {
            this.poolService = poolService;
            this.hitService = hitService;
        }

        public T LaunchProjectile<T>(Vector3 position, Vector3 launchVelocity, int damage, Vector3 grav, bool useGravity = false)
            where T : class, IProjectile
        {
            var projectile = poolService.Get<T>();
            var rotation = Quaternion.LookRotation(launchVelocity);
            projectile.Init(position, rotation, launchVelocity, damage, grav, useGravity);

            projectile.OnDropped += OnProjectileDropped;
            projectile.OnHitWith += OnProjectileHit;

            return projectile;
        }

        private void OnProjectileDropped(IDroppable dropped)
        {
            dropped.OnDropped -= OnProjectileDropped;

            if (dropped is not IProjectile proj) return;
            
            proj.OnHitWith -= OnProjectileHit;
            poolService.Return(proj);
        }

        private void OnProjectileHit(IHit hit, Collider other)
        {
            if (hit is IDamage damage)
                hitService.ReportHit(other, damage.Damage);
        }
    }
}