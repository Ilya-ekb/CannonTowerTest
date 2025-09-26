using UnityEngine;

namespace Services.Interfaces
{
    public interface ICollisionRegistry
    {
        void Register(IHitHandler hitHandler);
        void Unregister(IHitHandler collider);
        IHitHandler Resolve(Collider collider);
    }
}