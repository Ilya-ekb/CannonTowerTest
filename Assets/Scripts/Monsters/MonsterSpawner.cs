using Core;
using Services;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Monsters
{
    public class MonsterSpawner : UpdateableBehaviour
    {
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private Transform moveToTarget;
        
        private ISpawnService spawnService;
        private IPoolService poolService;
        private ICollisionRegistry collisionRegistry;
        private ICooldownService cooldownService;
        private ITargetRegistry targetRegistry;

        [Inject]
        public void Construct(ISpawnService spawner, IPoolService pool, ICollisionRegistry collisionReg, ITargetRegistry targetReg)
        {
            spawnService = spawner;
            poolService = pool;
            cooldownService = new CooldownService(spawnInterval, true);
            collisionRegistry = collisionReg;
            targetRegistry = targetReg;
        }

        public override void OnUpdate(float deltaTime)
        {
            if(!cooldownService.IsIntervalReached(deltaTime)) return;
            var monster = spawnService.Spawn<Monster>(transform.position, transform.rotation);
            monster.Init(moveToTarget);
            collisionRegistry.Register(monster);
            targetRegistry.Register(monster);
            if(monster is IDroppable droppable)
                droppable.OnDropped += OnMonsterDropped;
        }

        private void OnMonsterDropped(IDroppable droppable)
        {
            droppable.OnDropped -= OnMonsterDropped;
            var monster = droppable as Monster;
            collisionRegistry.Unregister(monster);
            targetRegistry.Unregister(monster);
            poolService.Return(monster);
        }
    }
}