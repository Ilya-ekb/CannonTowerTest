using Core;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IProjectileLifecycleService
    {
        T LaunchProjectile<T>(Vector3 position,
            Vector3 launchVelocity,
            int damage,
            Vector3 grav,
            bool useGravity = false)
            where T : class, IProjectile;
    }
}