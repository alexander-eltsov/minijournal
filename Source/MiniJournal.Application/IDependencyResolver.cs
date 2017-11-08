using System;

namespace Infotecs.MiniJournal.Application
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
    }
}
