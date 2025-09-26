using Core;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class SpawnService : ISpawnService
    {
        private readonly IPoolService pool;

        public SpawnService(IPoolService pool)
        {
            this.pool = pool;
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation) where T : class, IPoolable
        {
            var obj = pool.Get<T>();
            var monoBeh = obj as MonoBehaviour;
            if (monoBeh)
            {
                monoBeh.transform.position = position;
                monoBeh.transform.rotation = rotation;
            }
            return obj;
        }
    }
}