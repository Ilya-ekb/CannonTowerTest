using System.Collections.Generic;
using Monsters;
using UnityEngine;


public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile prefab;
    private readonly Queue<Projectile> pool = new();

    public Projectile Spawn(Vector3 position, Vector3 velocity, TowerConfig config)
    {
        var p = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        p.transform.position = position;
        p.gameObject.SetActive(true);
        p.Launch(config.projectileDamage, velocity, config.gravity, config.aimingType == AimingType.Parabolic, config.projectileLifeTime, this);
        return p;
    }
    
    public void Despawn(Projectile p)
    {
        p.ResetState();
        p.gameObject.SetActive(false);
        pool.Enqueue(p);
    }
}