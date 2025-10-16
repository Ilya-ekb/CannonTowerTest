using UnityEngine;

[CreateAssetMenu(menuName = "Configs/TowerConfig")]
public class TowerConfig : ScriptableObject
{
    [Header("Targeting")] public float findTargetDistance = 25f;

    [Header("Projectile")] public float projectileSpeed = 40f;
    public float shootInterval = 1.25f;
    public int projectileDamage = 10;
    public float projectileLifeTime = 3f;
    public Vector3 gravity = new(0, -9.81f, 0);

    [Header("Aiming")] public float rotateSpeed = 180f;
    public float aimThresholdDegrees = 3f;

    [Header("Mode")] public AimingType aimingType = AimingType.None;
    
}

public enum AimingType
{
    None,
    Predictive,
    Parabolic
}