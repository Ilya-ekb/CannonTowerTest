using Core;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Projectiles
{
    [RequireComponent(typeof(TransformMover))]
    public class GuidedProjectile : BaseProjectile
    {
        [SerializeField] private float homingRadius = 10f;
        private ITargetService targetService;

        [Inject]
        public void Construct(ITargetService targetServ)
        {
            targetService = targetServ;
        }
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            var target = FindClosestTarget();
            if (target is null) return;
            Vector3 toTarget = (target.Position - transform.position).normalized;
            mover.SetDirection(toTarget);
        }

        private IShootTarget FindClosestTarget()
        {
            var closest = targetService.GetTarget(transform.position, homingRadius);
            return closest;
        }
    }
}