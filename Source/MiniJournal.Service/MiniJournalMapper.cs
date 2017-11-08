using System.Collections;
using System.Collections.Generic;
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

        private static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<HeaderData, Header>();
            AutoMapper.Mapper.CreateMap<Header, HeaderData>();

            AutoMapper.Mapper.CreateMap<Comment, CommentData>();
            AutoMapper.Mapper.CreateMap<CommentData, Comment>();

            AutoMapper.Mapper.CreateMap<ArticleData, Article>()
                .ForMember(
                    article => article.Comments,
                    options => options.MapFrom(articleData => articleData.Comments));

            AutoMapper.Mapper.CreateMap<Article, ArticleData>();
        }
    }
}