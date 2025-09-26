using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class ClosestTargetService : ITargetService
    {
        private readonly ITargetRegistry registry;

        public ClosestTargetService(ITargetRegistry registry)
        {
            this.registry = registry;
        }

        public IShootTarget GetTarget(Vector3 from, float radius)
        {
            IShootTarget closest = null;
            var minDist = float.MaxValue;

            foreach (var t in registry.All)
            {
                if (!t.IsAlive) continue;

                var dist = Vector3.Distance(from, t.Position);
                if (dist < radius && dist < minDist)
                {
                    minDist = dist;
                    closest = t;
                }
            }

            return closest;
        }
    }
}