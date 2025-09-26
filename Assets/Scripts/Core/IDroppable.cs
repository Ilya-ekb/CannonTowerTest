using System;

namespace Core
{
    public interface IDroppable
    {
        event Action<IDroppable> OnDropped;
    }
}