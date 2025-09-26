using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine;

namespace Services.Collisions
{
    public class CollisionRegistry : ICollisionRegistry
    {
        private readonly Dictionary<Collider, IHitHandler> map = new(1024);

        public void Register(IHitHandler hitHandler)
        {
            if (hitHandler is not null && hitHandler.HitCollider)
                map[hitHandler.HitCollider] = hitHandler;
        }

        public void Unregister(IHitHandler hitHandler)
        {
            if (hitHandler.HitCollider)
                map.Remove(hitHandler.HitCollider);
        }

        public IHitHandler Resolve(Collider collider)
        {
            return collider ? map.GetValueOrDefault(collider) : null;
        }
    }
}