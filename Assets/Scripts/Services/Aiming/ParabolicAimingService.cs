using System;
using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services.Aiming
{
    public class ParabolicAimingService : IAimingService
    {
        private const int maxIterations = 6;
        private const float timeEpsilon = 0.01f;
     
        public Vector3 GetAimDirection(Transform shootPoint, IShootTarget target, float projectileSpeed, Vector3 gravity)
        {
            if (target == null || !target.IsAlive || projectileSpeed <= 0f)
                return shootPoint.forward * projectileSpeed;

            Vector3 origin = shootPoint.position;
            Vector3 targetPos = target.Position;
            Vector3 targetVel = target.Velocity;

            // Try to find a valid ballistic velocity for interception
            if (TrySolveBallisticIntercept(origin, targetPos, targetVel, gravity, projectileSpeed, out var launchVelocity))
                return launchVelocity;

            return Vector3.zero;
        }

        /// <summary>
        /// Iteratively solves for a valid ballistic interception velocity
        /// accounting for both target motion and gravity.
        /// </summary>
        private bool TrySolveBallisticIntercept(
            Vector3 origin,
            Vector3 targetPos,
            Vector3 targetVel,
            Vector3 gravity,
            float projectileSpeed,
            out Vector3 launchVelocity)
        {
            launchVelocity = Vector3.zero;

            float time = (targetPos - origin).magnitude / Mathf.Max(projectileSpeed, 0.01f);

            for (int i = 0; i < maxIterations; i++)
            {
                Vector3 predictedTarget = targetPos + targetVel * time;

                // Try to compute a ballistic arc to reach that point
                if (!TrySolveBallisticArc(origin, predictedTarget, gravity, projectileSpeed, out var candidate))
                    continue;

                // Re-estimate the projectile flight time for this new velocity
                float newTime = EstimateFlightTime(origin, predictedTarget, candidate);
                if (newTime <= 0f || float.IsInfinity(newTime))
                    return false;

                launchVelocity = candidate;

                // Check for convergence
                if (Mathf.Abs(newTime - time) < timeEpsilon)
                    return true;

                time = newTime;
            }

            return launchVelocity.sqrMagnitude > 0.001f;
        }

        private bool TrySolveBallisticArc(Vector3 origin, Vector3 target, Vector3 gravity, float speed, out Vector3 launchVelocity)
        {
            launchVelocity = Vector3.zero;

            Vector3 diff = target - origin;
            Vector3 diffXZ = new Vector3(diff.x, 0f, diff.z);
            float dx = diffXZ.magnitude;
            float dy = diff.y;

            float g = Mathf.Abs(gravity.y);
            float v2 = speed * speed;

            // Ballistic discriminant
            float underRoot = v2 * v2 - g * (g * dx * dx + 2f * dy * v2);
            if (underRoot < 0f)
                return false; // no real solution (target unreachable with this speed)

            float sqrt = Mathf.Sqrt(underRoot);

            float angleLow = Mathf.Atan2(v2 - sqrt, g * dx);
            float angleHigh = Mathf.Atan2(v2 + sqrt, g * dx);

            Vector3 dirXZ = diffXZ.normalized;

            Vector3 vLow = dirXZ * (speed * Mathf.Cos(angleLow)) + Vector3.up * (speed * Mathf.Sin(angleLow));
            float tLow = EstimateFlightTime(origin, target, vLow);

            Vector3 vHigh = dirXZ * (speed * Mathf.Cos(angleHigh)) + Vector3.up * (speed * Mathf.Sin(angleHigh));
            float tHigh = EstimateFlightTime(origin, target, vHigh);

            bool lowValid = tLow > 0 && float.IsFinite(tLow);
            bool highValid = tHigh > 0 && float.IsFinite(tHigh);

            // Prefer the shorter, lower arc (faster projectile)
            if (lowValid && (!highValid || tLow <= tHigh))
                launchVelocity = vLow;
            else if (highValid)
                launchVelocity = vHigh;
            else
                return false;

            return true;
        }

        private float EstimateFlightTime(Vector3 origin, Vector3 target, Vector3 velocity)
        {
            Vector3 disp = target - origin;
            float dx = new Vector2(disp.x, disp.z).magnitude;
            float horizSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            if (horizSpeed < 0.01f)
                return -1f;
            return dx / horizSpeed;
        }
    }
}