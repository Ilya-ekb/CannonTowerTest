using UnityEngine;

namespace Core
{
    public interface IShootTarget
    {
        Vector3 Position { get; }
        Vector3 Velocity { get; }
        bool IsAlive { get; }
    }
}