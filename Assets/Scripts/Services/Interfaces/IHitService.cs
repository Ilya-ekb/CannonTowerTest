using UnityEngine;

namespace Services.Interfaces
{
    public interface IHitService
    {
        void ReportHit(Collider target, int damage);
    }
}