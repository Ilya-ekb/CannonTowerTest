using System;
using System.Collections.Generic;
using Core;
using Services.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Services
{
    public class PoolService : IPoolService
    {
        private readonly Dictionary<Type, Queue<IPoolable>> pools = new();
        private readonly Dictionary<Type, IPoolable> prefabs = new();
        private readonly Transform parent;
        private readonly IObjectResolver resolver;

        public PoolService(IObjectResolver resolver, Transform parent)
        {
            this.resolver = resolver;
            this.parent = parent;
        }

        public void RegisterPrefab<T>(T prefab, int preload = 5) where T : class, IPoolable
        {
            var type = typeof(T);
            prefabs[type] = prefab;

            if (!pools.ContainsKey(type))
                pools[type] = new Queue<IPoolable>();

            for (int i = 0; i < preload; i++)
            {
                var obj = Object.Instantiate(prefab as MonoBehaviour, parent).GetComponent<T>();
                InitializeObject(obj);
                obj.OnReturnToPool();
                pools[type].Enqueue(obj);
            }
        }

        public T Get<T>() where T : class, IPoolable
        {
            var type = typeof(T);
            if (!pools.ContainsKey(type))
                throw new Exception($"No pool registered for {type}");

            var obj = pools[type].Count > 0
                ? (T)pools[type].Dequeue()
                : InitializeObject(Object.Instantiate(prefabs[type] as MonoBehaviour, parent).GetComponent<T>());

            obj.OnTakeFromPool();
            return obj;
        }

        public void Return<T>(T obj) where T : class, IPoolable
        {
            obj.OnReturnToPool();
            pools[obj.GetType()].Enqueue(obj);
        }

        public void Clear()
        {
            pools.Clear();
            prefabs.Clear();
        }

        private T InitializeObject<T>(T obj) where T : class, IPoolable
        {
            var injectable = (obj as MonoBehaviour);
            if (injectable != null)
                resolver.InjectGameObject(injectable.gameObject);
            return obj;
        }
    }
}