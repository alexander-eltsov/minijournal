using System.Collections.Generic;
using AutoMapper;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service
{
    public class MiniJournalMapper : IMapper
    {
        static MiniJournalMapper()
        {
            RegisterMappings();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map<TSource, TDestination>(source, destination);
        }

        private static void RegisterMappings()
        {
            Mapper.CreateMap<Header, HeaderData>();
            Mapper.CreateMap<HeaderData, Header>();

            Mapper.CreateMap<Comment, CommentData>();
            Mapper.CreateMap<CommentData, Comment>();

            Mapper.CreateMap<Article, ArticleData>();
            Mapper.CreateMap<ArticleData, Article>()
                .ConstructUsing(articleData =>
                {
                    var comments = Mapper.Map<IList<Comment>>(articleData.Comments);
                    var article = new Article(comments)
                    {
                        Id = articleData.Id,
                        Caption = articleData.Caption,
                        Text = articleData.Text
                    };
                    article.Comments.Each(comment => comment.Article = article);
                    return article;
                })
                .ForMember(
                    article => article.Comments,
                    options => options.Ignore());
        }
    }
}