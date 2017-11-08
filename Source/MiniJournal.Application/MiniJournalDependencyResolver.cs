using System;
using Autofac;

namespace Infotecs.MiniJournal.Application
{
    public class MiniJournalDependencyResolver : IDependencyResolver
    {
        private static IDependencyResolver instance;
        private static readonly object lockObject = new object();
        private readonly IContainer container;

        public MiniJournalDependencyResolver()
        {
            var containerBuilder = new AutofacContainerBuilder();
            container = containerBuilder.Build();
        }

        public static IDependencyResolver Instance()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new MiniJournalDependencyResolver();
                }
                return instance;
            }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
