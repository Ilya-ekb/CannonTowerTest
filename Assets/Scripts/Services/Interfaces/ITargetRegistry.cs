using System.Collections.Generic;
using Core;

namespace Services.Interfaces
{
    public interface ITargetRegistry
    {
        void Register(IShootTarget target);
        void Unregister(IShootTarget target);
        IReadOnlyCollection<IShootTarget> All { get; }
    }}