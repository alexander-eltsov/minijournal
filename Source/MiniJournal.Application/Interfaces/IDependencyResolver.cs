using System;

namespace Infotecs.MiniJournal.Application.Interfaces
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
    }
}
