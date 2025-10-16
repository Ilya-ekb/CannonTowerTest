using Monsters;
using UnityEngine;

public class GuidedProjectile : Projectile
{
    [SerializeField] private float findTargetRadius = 3f;
    private GameObject target;

    public override void ResetState()
    {
        target = null;
        base.ResetState();
    }

    protected override void Update()
    {
        if (target)
        {
            var dir = (target.transform.position - transform.position).normalized;
            velocity = dir * velocity.magnitude;
        }
        else
        {
            target = Monster.GetClosest(transform.position, findTargetRadius)?.gameObject;
        }
        base.Update();
    }
}