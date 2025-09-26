using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services.Aiming
{
    public class PredictiveAimingService : IAimingService
    {
        public Vector3 GetAimDirection(Transform shootPoint, IShootTarget target,
            float projectileSpeed, Vector3 gravity)
        {
            if (target == null || !target.IsAlive)
                return shootPoint.forward * projectileSpeed;

            Vector3 toTarget = target.Position - shootPoint.position;
            float distance = toTarget.magnitude;

            float time = distance / projectileSpeed;
            Vector3 futurePos = target.Position + target.Velocity * time;

            Vector3 dir = (futurePos - shootPoint.position).normalized;
            
            return dir * projectileSpeed;
        }
    }
}