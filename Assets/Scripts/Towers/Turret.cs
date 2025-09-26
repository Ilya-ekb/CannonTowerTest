using System;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class Turret
    {
        [SerializeField] private Transform turretBase;
        [SerializeField] private Transform barrelPivot;

        public void ApplyLaunchVelocityToTurret(Vector3 launchVelocity, float rotationSpeed, float dt)
        {
            if (launchVelocity.sqrMagnitude < Mathf.Epsilon) return;

            Vector3 horizDir = new Vector3(launchVelocity.x, 0f, launchVelocity.z);
            if (horizDir.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion targetYaw = Quaternion.LookRotation(horizDir.normalized, Vector3.up);
                turretBase.rotation = Quaternion.RotateTowards(
                    turretBase.rotation,
                    targetYaw,
                    rotationSpeed * dt
                );
            }

            float horizSpeed = new Vector3(launchVelocity.x, 0f, launchVelocity.z).magnitude;
            float pitchRad = -Mathf.Atan2(launchVelocity.y, horizSpeed);
            float pitchDeg = Mathf.Rad2Deg * pitchRad;

            Vector3 localEuler = barrelPivot.localEulerAngles;

            float currentPitch = NormalizeAngle(localEuler.x);
            float targetPitch = Mathf.LerpAngle(currentPitch, pitchDeg, rotationSpeed * dt);
            barrelPivot.localEulerAngles = new Vector3(targetPitch, localEuler.y, localEuler.z);
        }

        private float NormalizeAngle(float a)
        {
            a = (a + 180f) % 360f;
            if (a < 0) a += 360f;
            return a - 180f;
        }
    }
}