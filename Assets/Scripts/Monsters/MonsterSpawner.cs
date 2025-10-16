using Configs;
using Core;
using Services;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Monsters
{
    public class MonsterSpawner : UpdateableBehaviour
    {
        [SerializeField] private MonsterSpawnConfig config;
        [SerializeField] private Transform moveToTarget;
        
        private IPoolService poolService;
        private ICollisionRegistry collisionRegistry;
        private ICooldownService cooldownService;
        private ITargetRegistry targetRegistry;

        [Inject]
        public void Construct(IPoolService poolServ, ICollisionRegistry collisionReg, ITargetRegistry targetReg)
        {
            poolService = poolServ;
            cooldownService = new CooldownService(config.spawnInterval, true);
            collisionRegistry = collisionReg;
            targetRegistry = targetReg;
        }

        public override void OnUpdate(float deltaTime)
        {
            if(!cooldownService.IsIntervalReached(deltaTime)) return;
            cooldownService.SetInterval(config.spawnInterval);
            var monster = poolService.Get<Monster>();
            monster.Init(transform.position, transform.rotation, moveToTarget, config.monsterHp, config.monsterSpeed);
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