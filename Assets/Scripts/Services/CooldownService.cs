using Services.Interfaces;

namespace Services
{
    public class CooldownService : ICooldownService
    {
        private float lastShotTime;

        private readonly float cooldown;

        public CooldownService(float cooldown, bool reachOnStart = false)
        {
            this.cooldown = cooldown;
            if(reachOnStart)
                lastShotTime = cooldown;
        }

        public bool IsIntervalReached(float step)
        {
            lastShotTime += step;
            if (lastShotTime < cooldown) return false;
            lastShotTime = 0;
            return true;
        }
    }
}