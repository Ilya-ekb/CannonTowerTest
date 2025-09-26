using System;
using UnityEngine;

namespace Core
{
    public interface IHit
    {
        event Action<IHit, Collider> OnHitWith;
    }
}