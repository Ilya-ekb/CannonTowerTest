using TriInspector;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "Configs/TowerConfig")]
    public class TowerConfig : ScriptableObject
    {
        public float shootInterval = 0.5f;
        public float projectileSpeed = 10f;
        public float findTargetDistance = 10f;
        public float rotateSpeed = 10f;
        public int projectileDamage = 10;
        public AimingType aimingType;
        [ShowIf(nameof(aimingType), AimingType.Parabolic)]
        public Vector3 gravity = Physics.gravity;
    }

    public enum AimingType
    {
        Instant,
        Smooth,
        Predictive,
        Parabolic,
    }
}