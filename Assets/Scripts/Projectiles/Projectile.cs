using Monsters;
using UnityEngine;

/// <summary>
/// Simple projectile with gravity and pooling support.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public int Damage { get; private set; }
    protected Vector3 velocity;
    private Vector3 gravity;
    private bool useGravity;
    private float lifetime;
    private float timer;
    private ProjectilePool pool;

    public void Launch( int dmg, Vector3 launchVel, Vector3 grav, bool useGrav, float lTime, ProjectilePool projectilePool)
    {
        Damage = dmg;
        velocity = launchVel;
        gravity = grav;
        useGravity = useGrav;
        lifetime = lTime;
        timer = 0f;
        pool = projectilePool;
    }

    public virtual void ResetState()
    {
        velocity = Vector3.zero;
        timer = 0f;
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            pool.Despawn(this);
            return;
        }

        if (useGravity)
            velocity += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Target"))
            return;

        if (other.TryGetComponent<Monster>(out var target))
            target.OnHit(this);

        pool.Despawn(this);
    }
}