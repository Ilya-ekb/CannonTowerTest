using Core;
using UnityEngine;
using Services.Interfaces;

namespace Services
{
    public class ProjectileLifecycleService : IProjectileLifecycleService
    {
        private readonly ISpawnService spawner;
        private readonly IPoolService poolService;
        private readonly IHitService hitService;

        public ProjectileLifecycleService(ISpawnService spawner, IPoolService poolService, IHitService hitService)
        {
            this.spawner = spawner;
            this.poolService = poolService;
            this.hitService = hitService;
        }

        public T LaunchProjectile<T>(Vector3 position, Vector3 launchVelocity, int damage, Vector3 grav, bool useGravity = false)
            where T : class, IProjectile
        {
            var projectile = spawner.Spawn<T>(position, Quaternion.LookRotation(launchVelocity));

            projectile.Init(launchVelocity, damage, grav, useGravity);

            projectile.OnDropped += OnProjectileDropped;
            projectile.OnHitWith += OnProjectileHit;

            return projectile;
        }

        private void OnProjectileDropped(IDroppable dropped)
        {
            dropped.OnDropped -= OnProjectileDropped;

            if (dropped is IProjectile proj)
            {
                proj.OnHitWith -= OnProjectileHit;
                poolService.Return(proj);
            }
        }

        private void OnProjectileHit(IHit hit, Collider other)
        {
            if (hit is IDamage damage)
                hitService.ReportHit(other, damage.Damage);
        }
    }
}