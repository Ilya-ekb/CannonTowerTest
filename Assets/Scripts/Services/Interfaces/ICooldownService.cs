namespace Services.Interfaces
{
    public interface ICooldownService
    {
        bool IsIntervalReached(float step);
    }
}