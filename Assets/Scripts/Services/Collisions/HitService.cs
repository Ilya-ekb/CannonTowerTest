using Services.Interfaces;
using UnityEngine;

namespace Services.Collisions
{
    public class HitService : IHitService
    {
        private readonly ICollisionRegistry registry;

        public HitService(ICollisionRegistry registry)
        {
            this.registry = registry;
        }
        
        public void ReportHit(Collider target, int damage)
        {
            var handler = registry.Resolve(target);
            handler?.OnHit(damage);
        }
    }
}