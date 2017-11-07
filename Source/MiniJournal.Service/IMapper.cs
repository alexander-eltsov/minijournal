using System;

namespace Infotecs.MiniJournal.Service
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
