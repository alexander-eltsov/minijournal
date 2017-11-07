using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service
{
    public class MiniJournalMapper : IMapper
    {
        public MiniJournalMapper()
        {
            RegisterMappings();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }

        private void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<ArticleData, Article>();
            AutoMapper.Mapper.CreateMap<Article, ArticleData>();
        }
    }
}