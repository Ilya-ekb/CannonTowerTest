using System;
using Core;
using Services.Interfaces;
using UnityEngine;

namespace Monsters
{
    [RequireComponent(typeof(RigidbodyMover))]
    public class Monster : UpdateableBehaviour, IPoolable, IShootTarget, IHitHandler, IDroppable
    {
        public event Action<IDroppable> OnDropped;
        public Vector3 Position => transform.position;
        public Vector3 Velocity => mover.Velocity;
        public bool IsAlive => hp > 0;
        public Collider HitCollider { get; private set; }

        private int hp;

        private Mover mover;
        private Transform moveToTarget;

        private const float reachTargetThreshold = 0.2f;

        public void Init(Vector3 position, Quaternion rotation, Transform target, int healthPoints, float speed)
        {
            hp = healthPoints;
            moveToTarget = target;
            transform.position = position;
            transform.rotation = rotation;
            var dir = (target.position - transform.position).normalized;
            if (!mover)
                mover = GetComponent<RigidbodyMover>();
            if (!HitCollider)
                HitCollider = GetComponentInChildren<Collider>();
            mover.Setup(speed, dir, position, rotation);
        }

        public void OnTakeFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!moveToTarget) return;
            if (Vector3.Distance(transform.position, moveToTarget.position) < reachTargetThreshold)
                Drop();
        }

        public void OnHit(int damage)
        {
            hp -= damage;
            if (hp <= 0)
                Drop();
        }

        private void Drop()
        {
            OnDropped?.Invoke(this);
        }
    }
}