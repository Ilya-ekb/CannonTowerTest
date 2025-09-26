using Projectiles;
using UnityEngine;

namespace Towers
{
    public class GuidedTower : BaseTower<GuidedProjectile>
    {
        protected override Vector3 GetLaunchVelocity(float deltaTime)
        {
            return shootPoint.forward * config.projectileSpeed;
        }
    }
}