using System;
using Monsters;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerConfig config;
        [SerializeField] private Turret turret;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private ProjectilePool projectilePool;

        private Monster currentTarget;
        private Cooldown reload;
        private IAiming aiming;

        private void Awake()
        {
            reload = new Cooldown(config.shootInterval);
            aiming = config.aimingType switch
            {
                AimingType.Parabolic => new ParabolicAiming(),
                AimingType.Predictive => new PredictiveAiming(),
                AimingType.None => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void Update()
        {
            // --- Find target ---
            currentTarget = Monster.GetClosest(transform.position, config.findTargetDistance);
            if (!currentTarget) return;

            // --- Aim ---
            var dir = aiming?.GetAimDirection(shootPoint, currentTarget, config.projectileSpeed, config.gravity) ??
                      shootPoint.forward;
            Vector3 launchVelocity = dir * config.projectileSpeed;
            if (turret.IsAlive)
                turret.ApplyLaunchVelocityToTurret(launchVelocity, config.rotateSpeed, Time.deltaTime);

            // --- Fire ---
            if (reload.Ready && (!turret.IsAlive || turret.IsAimedAt(launchVelocity, config.aimThresholdDegrees)))
            {
                projectilePool?.Spawn(shootPoint.position, launchVelocity, config);
                reload.Reset();
            }

            reload.Update();
        }
    }
}