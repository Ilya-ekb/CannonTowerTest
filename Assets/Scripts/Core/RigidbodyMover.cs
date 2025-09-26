using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMover : Mover
    {
        private Rigidbody rb;

        public override void Setup(float speed,
            Vector3 dir,
            Vector3 startPos,
            Quaternion startRot,
            Vector3 grav,
            bool useGrav = false)
        {
            base.Setup(speed, dir, startPos, startRot, grav, useGrav);
            if (!rb) rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.position = startPos;
            rb.rotation = startRot;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            var newPos = rb.position + Velocity * deltaTime;
            rb.MovePosition(newPos);
            if (Velocity.sqrMagnitude > 0.0001f)
                rb.MoveRotation(Quaternion.LookRotation(Velocity));
        }
    }
}