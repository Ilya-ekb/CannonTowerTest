using Core;
using UnityEngine;

namespace Services.Interfaces
{
    public interface ITargetService
    {
        IShootTarget GetTarget(Vector3 position, float maxDistance);
    }
}