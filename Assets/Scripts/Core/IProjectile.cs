using UnityEngine;

namespace Core
{
    public interface IProjectile : IDroppable, IHit, IDamage, IPoolable
    {
        void Init(Vector3 position, Quaternion rotation, Vector3 launchVelocity, int damage, Vector3 grav, bool useGravity = false);
    }
}