using System.Collections.Generic;
using Core;
using Services.Interfaces;

namespace Services
{
    public class UpdateService : IUpdateService
    {
        private readonly List<IUpdateable> updateablesToAdd = new List<IUpdateable>();
        private readonly List<IUpdateable> fixedUpdateablesToAdd = new List<IUpdateable>();
        private readonly List<IUpdateable> lateUpdateablesToAdd = new List<IUpdateable>();
        private readonly List<IUpdateable> updateables = new List<IUpdateable>();
        private readonly List<IUpdateable> fixedUpdateables = new List<IUpdateable>();
        private readonly List<IUpdateable> lateUpdateables = new List<IUpdateable>();
        private readonly List<IUpdateable> updateablesToRemove = new List<IUpdateable>();
        private readonly List<IUpdateable> fixedUpdateablesToRemove = new List<IUpdateable>();
        private readonly List<IUpdateable> lateUpdateablesToRemove = new List<IUpdateable>();

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
            foreach (var updateable in updateables)
                updateable.OnUpdate(dt);
        }

        public void TickFixedUpdate(float dt)
        {
            ApplyFixedUpdates();
            foreach (var updateable in fixedUpdateables)
                updateable.OnUpdate(dt);
        }

        public void TickLateUpdate(float dt)
        {
            ApplyLateUpdates();
            foreach (var updateable in lateUpdateables)
                updateable.OnUpdate(dt);
        }

        private void ApplyUpdates()
        {
            foreach (var updateable in updateablesToAdd)
                updateables.Add(updateable);
            updateablesToAdd.Clear();
            foreach (var updateable in updateablesToRemove)
                updateables.Remove(updateable);
            updateablesToRemove.Clear();
        }

        private void ApplyFixedUpdates()
        {
            foreach (var updateable in fixedUpdateablesToAdd)
                fixedUpdateables.Add(updateable);
            fixedUpdateablesToAdd.Clear();
            foreach (var updateable in fixedUpdateablesToRemove)
                fixedUpdateables.Remove(updateable);
            fixedUpdateablesToRemove.Clear();
        }

        private void ApplyLateUpdates()
        {
            foreach (var updateable in lateUpdateablesToAdd)
                lateUpdateables.Add(updateable);
            lateUpdateablesToAdd.Clear();
            foreach (var updateable in lateUpdateablesToRemove)
                lateUpdateables.Remove(updateable);
            lateUpdateablesToRemove.Clear();
        }
    }
}