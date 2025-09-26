using System.Collections.Generic;
using Core;
using Services.Interfaces;

namespace Services
{
    public class UpdateService : IUpdateService
    {
        private readonly List<IUpdateable> updateablesToAdd         = new(initCapacity);
        private readonly List<IUpdateable> fixedUpdateablesToAdd    = new(initCapacity);
        private readonly List<IUpdateable> lateUpdateablesToAdd     = new(initCapacity);
        private readonly List<IUpdateable> updateables              = new(initCapacity);
        private readonly List<IUpdateable> fixedUpdateables         = new(initCapacity);
        private readonly List<IUpdateable> lateUpdateables          = new(initCapacity);
        private readonly List<IUpdateable> updateablesToRemove      = new(initCapacity);
        private readonly List<IUpdateable> fixedUpdateablesToRemove = new(initCapacity);
        private readonly List<IUpdateable> lateUpdateablesToRemove  = new(initCapacity);

        private const int initCapacity = 256;

        public void RegisterToUpdate(IUpdateable updateable)
        {
            updateablesToAdd.Add(updateable);
        }

        public void UnregisterFromUpdate(IUpdateable updateable)
        {
            updateablesToRemove.Add(updateable);
        }

        public void RegisterToFixedUpdate(IUpdateable updateable)
        {
            fixedUpdateablesToAdd.Add(updateable);
        }

        public void UnregisterFromFixedUpdate(IUpdateable updateable)
        {
            fixedUpdateablesToRemove.Add(updateable);
        }

        public void RegisterToLateUpdate(IUpdateable updateable)
        {
            lateUpdateablesToAdd.Add(updateable);
        }

        public void UnregisterFromLateUpdate(IUpdateable updateable)
        {
            lateUpdateablesToRemove.Add(updateable);
        }
        
        public void TickUpdate(float dt)
        {
            ApplyUpdates();
            for (var index = 0; index < updateables.Count; index++)
                updateables[index].OnUpdate(dt);
        }

        public void TickFixedUpdate(float dt)
        {
            ApplyFixedUpdates();
            for (var index = 0; index < fixedUpdateables.Count; index++)
                fixedUpdateables[index].OnUpdate(dt);
        }

        public void TickLateUpdate(float dt)
        {
            ApplyLateUpdates();
            for (var index = 0; index < lateUpdateables.Count; index++)
                lateUpdateables[index].OnUpdate(dt);
        }

        private void ApplyUpdates()
        {
            for (var index = 0; index < updateablesToAdd.Count; index++)
            {
                updateables.Add(updateablesToAdd[index]);
            }

            updateablesToAdd.Clear();
            for (var index = 0; index < updateablesToRemove.Count; index++)
            {
                updateables.Remove(updateablesToRemove[index]);
            }

            updateablesToRemove.Clear();
        }

        private void ApplyFixedUpdates()
        {
            for (var index = 0; index < fixedUpdateablesToAdd.Count; index++)
            {
                fixedUpdateables.Add(fixedUpdateablesToAdd[index]);
            }

            fixedUpdateablesToAdd.Clear();
            for (var index = 0; index < fixedUpdateablesToRemove.Count; index++)
            {
                fixedUpdateables.Remove(fixedUpdateablesToRemove[index]);
            }

            fixedUpdateablesToRemove.Clear();
        }

        private void ApplyLateUpdates()
        {
            for (var index = 0; index < lateUpdateablesToAdd.Count; index++)
            {
                lateUpdateables.Add(lateUpdateablesToAdd[index]);
            }

            lateUpdateablesToAdd.Clear();
            for (var index = 0; index < lateUpdateablesToRemove.Count; index++)
            {
                lateUpdateables.Remove(lateUpdateablesToRemove[index]);
            }

            lateUpdateablesToRemove.Clear();
        }
    }
}