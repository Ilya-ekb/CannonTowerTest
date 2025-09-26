using Monsters;
using Projectiles;
using Services;
using Services.Collisions;
using Services.Interfaces;
using Services.Targeting;
using Towers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameInstaller : Installer
    {
        [SerializeField] private CannonProjectile cannonProjectilePrefab;
        [SerializeField] private GuidedProjectile guidedProjectilePrefab;
        [SerializeField] private Monster monsterPrefab;
        [SerializeField] private int targetFrameRate = -1;

        public override void Install(IContainerBuilder builder)
        {
            Application.targetFrameRate = targetFrameRate;
            builder.Register<TargetRegistry>(Lifetime.Scoped).As<ITargetRegistry>();
            builder.Register<ClosestTargetService>(Lifetime.Scoped).As<ITargetService>();
            builder.Register<SpawnService>(Lifetime.Scoped).As<ISpawnService>();
            builder.Register<ProjectileLifecycleService>(Lifetime.Scoped).As<IProjectileLifecycleService>();
            builder.Register<CollisionRegistry>(Lifetime.Scoped).As<ICollisionRegistry>();
            builder.Register<HitService>(Lifetime.Scoped).As<IHitService>();
            builder.Register<PoolService>(Lifetime.Scoped).As<IPoolService>().WithParameter(transform).AsSelf();

            builder.RegisterBuildCallback(resolver =>
            {
                var poolService = resolver.Resolve<PoolService>();
                poolService.RegisterPrefab(cannonProjectilePrefab);
                poolService.RegisterPrefab(guidedProjectilePrefab);
                poolService.RegisterPrefab(monsterPrefab);
            });

            builder.RegisterComponentInHierarchy<CannonTower>();
            builder.RegisterComponentInHierarchy<GuidedTower>();
            builder.RegisterComponentInHierarchy<MonsterSpawner>();
        }
    }
}