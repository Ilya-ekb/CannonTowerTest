using System;
using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services.Aiming
{
    public class ParabolicAimingService : IAimingService
    {
        private const int maxIterations = 5;
        private const float timeEpsilon = 0.05f;
        public Vector3 GetAimDirection(Transform shootPoint, IShootTarget target,
            float projectileSpeed, Vector3 gravity)
        {
            if (target == null || !target.IsAlive)
                return shootPoint.forward * projectileSpeed;

            Vector3 origin = shootPoint.position;
            Vector3 targetPos = target.Position;
            Vector3 targetVel = target.Velocity;

            if (TryGetPredictedLaunch(origin, targetPos, targetVel, gravity, projectileSpeed, out var launchVelocity))
                return launchVelocity;

            Vector3 toTarget = targetPos - origin;
            float approxTime = toTarget.magnitude / Mathf.Max(0.01f, projectileSpeed);
            Vector3 futurePos = targetPos + target.Velocity * approxTime;
            return (futurePos - origin).normalized * projectileSpeed;
        }
        
        private bool TryGetPredictedLaunch(
            Vector3 origin,
            Vector3 targetPos,
            Vector3 targetVel,
            Vector3 gravity,
            float speed,
            out Vector3 launchVelocity)
        {
            launchVelocity = Vector3.zero;

            float time = Vector3.Distance(origin, targetPos) / Mathf.Max(speed, 0.01f);

            for (int i = 0; i < maxIterations; i++)
            {
                Vector3 predicted = targetPos + targetVel * time;

                if (!TryGetLaunchVelocity(origin, predicted, gravity, speed, out var candidate))
                    return false;

                float newTime = EstimateFlightTime(origin, predicted, candidate);

                if (newTime <= 0 || float.IsInfinity(newTime))
                    return false;

                launchVelocity = candidate;

                if (Mathf.Abs(newTime - time) < timeEpsilon)
                    return true;

                time = newTime;
            }

            return launchVelocity != Vector3.zero;
        }
        
        private bool TryGetLaunchVelocity(Vector3 origin, Vector3 targetPos, Vector3 gravity, float speed, out Vector3 launchVelocity)
        {
            launchVelocity = Vector3.zero;

            Vector3 disp = targetPos - origin;
            Vector3 dispXZ = new Vector3(disp.x, 0f, disp.z);
            float dx = dispXZ.magnitude;
            float dy = disp.y;

            float g = Mathf.Abs(gravity.y);
            float v2 = speed * speed;
            float term = v2 * v2 - g * (g * dx * dx + 2 * dy * v2);

            if (term < 0f) return false;

            float sqrt = Mathf.Sqrt(term);

            float angleLow = Mathf.Atan2(v2 - sqrt, g * dx);
            float angleHigh = Mathf.Atan2(v2 + sqrt, g * dx);

            Vector3 dirXZ = dispXZ.normalized;

            Vector3 vLow = dirXZ * (speed * Mathf.Cos(angleLow)) + Vector3.up * (speed * Mathf.Sin(angleLow));
            float tLow = EstimateFlightTime(origin, targetPos, vLow);

            Vector3 vHigh = dirXZ * (speed * Mathf.Cos(angleHigh)) + Vector3.up * (speed * Mathf.Sin(angleHigh));
            float tHigh = EstimateFlightTime(origin, targetPos, vHigh);

            bool lowValid = tLow > 0 && float.IsFinite(tLow);
            bool highValid = tHigh > 0 && float.IsFinite(tHigh);

            if (lowValid && (!highValid || tLow <= tHigh))
                launchVelocity = vLow;
            else if (highValid)
                launchVelocity = vHigh;
            else
                return false;

            return true;
        }

        private float EstimateFlightTime(Vector3 origin, Vector3 target, Vector3 launchVel)
        {
            Vector3 disp = target - origin;
            float dx = new Vector2(disp.x, disp.z).magnitude;
            float horizSpeed = new Vector2(launchVel.x, launchVel.z).magnitude;
            if (horizSpeed < 0.01f) return -1;
            return dx / horizSpeed;
        }
    }
}