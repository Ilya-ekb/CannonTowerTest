using System.Collections.Generic;
using Core;
using Services.Interfaces;

namespace Services.Targeting
{
    public class TargetRegistry : ITargetRegistry
    {
        private readonly HashSet<IShootTarget> targets = new(1024);

        public void Register(IShootTarget target) => targets.Add(target);
        public void Unregister(IShootTarget target) => targets.Remove(target);
        public IReadOnlyCollection<IShootTarget> All => targets;
    }
}