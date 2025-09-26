using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services.Aiming
{
    public class InstantAimingService : IAimingService
    {
        public Vector3 GetAimDirection(Transform shootPoint, IShootTarget target,
            float projectileSpeed, Vector3 gravity)
        {
            if (target is not { IsAlive: true })
                return shootPoint.forward * projectileSpeed;

            Vector3 dir = (target.Position - shootPoint.position).normalized;

            return dir * projectileSpeed;
        }
    }
}