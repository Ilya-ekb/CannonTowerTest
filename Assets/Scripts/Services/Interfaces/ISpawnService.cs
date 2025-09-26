using Core;
using UnityEngine;

namespace Services.Interfaces
{
    public interface ISpawnService
    {
        T Spawn<T>(Vector3 position, Quaternion rotation) where T : class, IPoolable;
    }
}