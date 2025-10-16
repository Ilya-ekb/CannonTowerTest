namespace Services.Interfaces
{
    public interface ICooldownService
    {
        void SetInterval(float interval);
        bool IsIntervalReached(float step);
    }
}