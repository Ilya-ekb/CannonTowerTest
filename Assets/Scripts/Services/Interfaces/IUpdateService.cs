using Core;

namespace Services.Interfaces
{
    public interface IUpdateService
    {
        void RegisterToUpdate(IUpdateable updateable);
        void UnregisterFromUpdate(IUpdateable updateable);
        void RegisterToFixedUpdate(IUpdateable updateable);
        void UnregisterFromFixedUpdate(IUpdateable updateable);
        void RegisterToLateUpdate(IUpdateable updateable);
        void UnregisterFromLateUpdate(IUpdateable updateable);
        void TickUpdate(float dt);
        void TickFixedUpdate(float dt);
        void TickLateUpdate(float dt);
    }
}