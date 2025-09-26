using UnityEngine;

namespace Core
{
    public abstract class Mover : UpdateableBehaviour
    {
        public Vector3 Velocity { get; private set; }

        private bool useGravity;
        private Vector3 gravity;

        private float moveSpeed;
        private Vector3 direction;


        public virtual void Setup(
            float speed, 
            Vector3 dir,
            Vector3 startPosition,
            Quaternion startRotation,
            Vector3 grav,
            bool useGrav = false)
        {
            moveSpeed = speed;
            direction = dir;
            Velocity = dir * moveSpeed;
            transform.position = startPosition;
            transform.rotation = startRotation;
            useGravity = useGrav;
            gravity = grav;
        }

        public void SetDirection(Vector3 dir)
        {
            direction = dir;
            Velocity = direction * moveSpeed;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (useGravity)
                Velocity += gravity * deltaTime;
        }
    }
}