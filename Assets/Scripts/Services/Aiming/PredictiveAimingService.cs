using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services.Aiming
{
    public class PredictiveAimingService : IAimingService
    {
        public Vector3 GetAimDirection(Transform shootPoint, IShootTarget target, float projectileSpeed, Vector3 gravity)
        {
            if (target == null || !target.IsAlive || projectileSpeed <= 0f)
                return Vector3.zero;

            Vector3 shooterPos = shootPoint.position;
            Vector3 targetPos = target.Position;
            Vector3 targetVel = target.Velocity;

            Vector3 relPos = targetPos - shooterPos;
            Vector3 relVel = targetVel;
            
            // Precompute dot products
            float rr = Vector3.Dot(relPos, relPos);   // |r|²
            float vv = Vector3.Dot(relVel, relVel);   // |v|²
            float rv = Vector3.Dot(relPos, relVel);   // r·v

            // Quadratic equation: (v·v - s²)t² + 2(r·v)t + (r·r) = 0
            float a = vv - projectileSpeed * projectileSpeed;
            float b = 2f * rv;
            float c = rr;

            float t;

            if (Mathf.Abs(a) < Mathf.Epsilon)
            {
                // Linear case: target speed is small or close to projectile speed
                t = (Mathf.Abs(b) > Mathf.Epsilon) ? -c / b : 0f;
                if (t < 0f) t = 0f;
            }
            else
            {
                float disc = b * b - 4f * a * c;
                if (disc < 0f)
                {
                    // No real solution: target moves too fast or is moving away
                    return relPos.normalized * projectileSpeed;
                }

                float sqrtD = Mathf.Sqrt(disc);
                float t1 = (-b + sqrtD) / (2f * a);
                float t2 = (-b - sqrtD) / (2f * a);

                // Pick the smallest positive time
                t = Mathf.Min(t1, t2);
                if (t < Mathf.Epsilon) t = Mathf.Max(t1, t2);
                if (t < Mathf.Epsilon) t = 0f;
            }

            // Predict target's future position
            Vector3 futurePos = targetPos + targetVel * t;
            Vector3 aimDir = (futurePos - shooterPos).normalized;

            return aimDir * projectileSpeed;
        }
    }
}