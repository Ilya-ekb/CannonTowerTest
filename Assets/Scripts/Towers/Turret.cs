using System;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class Turret
    {
        public bool IsAlive => turretBase && barrelPivot;
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform barrelPivot;

        public void ApplyLaunchVelocityToTurret(Vector3 launchVelocity, float rotationSpeed, float dt)
        {
            if (launchVelocity.sqrMagnitude < Mathf.Epsilon) return;

            Vector3 horizDir = new Vector3(launchVelocity.x, 0f, launchVelocity.z);
            float horizSpeed = horizDir.magnitude;

            if (horizDir.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion targetYaw = Quaternion.LookRotation(horizDir.normalized, Vector3.up);
                turretBase.rotation = Quaternion.RotateTowards(
                    turretBase.rotation,
                    targetYaw,
                    rotationSpeed * dt
                );
            }

            float targetPitchDeg = -Mathf.Rad2Deg * Mathf.Atan2(launchVelocity.y, horizSpeed);

            Vector3 localEuler = barrelPivot.localEulerAngles;
            float currentPitch = NormalizeAngle(localEuler.x);

            float newPitch = Mathf.MoveTowardsAngle(
                currentPitch,
                targetPitchDeg,
                rotationSpeed * dt
            );

            barrelPivot.localEulerAngles = new Vector3(newPitch, localEuler.y, localEuler.z);
        }

        public bool IsAimedAt(Vector3 launchVelocity, float maxAngle)
        {
            return Vector3.Angle(barrelPivot.forward, launchVelocity.normalized) <= maxAngle;
        }

        private float NormalizeAngle(float a)
        {
            a = (a + 180f) % 360f;
            if (a < 0) a += 360f;
            return a - 180f;
        }
    }
}