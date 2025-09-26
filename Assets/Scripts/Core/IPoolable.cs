namespace Core
{
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
    }
}