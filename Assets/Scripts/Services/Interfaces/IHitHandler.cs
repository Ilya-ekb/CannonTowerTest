using UnityEngine;

namespace Services.Interfaces
{
    public interface IHitHandler
    {
        Collider HitCollider { get; }
        void OnHit(int damage);
    }
}