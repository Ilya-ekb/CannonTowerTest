using Core;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IAimingService
    {
        Vector3 GetAimDirection(
            Transform shootPoint,
            IShootTarget target,
            float projectileSpeed,
            Vector3 gravity);
    }
}