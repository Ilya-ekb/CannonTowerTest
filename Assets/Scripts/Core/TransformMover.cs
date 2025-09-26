using UnityEngine;

namespace Core
{
    public class TransformMover : Mover
    {
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            transform.position += Velocity * deltaTime;
            if (Velocity.sqrMagnitude > Mathf.Epsilon)
                transform.rotation = Quaternion.LookRotation(Velocity);
        }
    }
}