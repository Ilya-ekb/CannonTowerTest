using System;
using Core;
using UnityEngine;

namespace Projectiles
{
    public abstract class BaseProjectile : UpdateableBehaviour, IProjectile
    {
        public event Action<IDroppable> OnDropped;
        public event Action<IHit, Collider> OnHitWith;

        public int Damage { get; private set; }

        protected Mover mover;

        [SerializeField] private float lifeTime = 3f;
        private float currentLifeTime;


        public virtual void Init(Vector3 launchVelocity, int projectileDamage, Vector3 grav, bool useGravity = false)
        {
            Damage = projectileDamage;
            currentLifeTime = lifeTime;
            if (!mover)
                mover = GetComponent<Mover>();
            if (mover)
                mover.Setup(
                    launchVelocity.magnitude,
                    launchVelocity.normalized,
                    transform.position, 
                    transform.rotation, grav,
                    useGravity);
        }

        public override void OnUpdate(float deltaTime)
        {
            currentLifeTime -= deltaTime;
            if (currentLifeTime <= 0f)
            {
                Drop();
            }
        }

        protected virtual void Drop()
        {
            currentLifeTime = lifeTime;
            OnDropped?.Invoke(this);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnHitWith?.Invoke(this, other);
            Drop();
        }

        public virtual void OnTakeFromPool()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
}