using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;

namespace Infotecs.MiniJournal.Service
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository ArticleRepository { get; }

        public ArticleService(IArticleRepository articleRepository)
        {
            if (articleRepository == null)
            {
                throw new ArgumentNullException(nameof(articleRepository));
            }

            ArticleRepository = articleRepository;
        }

        public IEnumerable<ArticleData> GetAllArticles()
        {
            return ArticleRepository.GetArticles();
        }

        public bool CreateArticle(ArticleData article)
        {
            return ArticleRepository.CreateArticle(article);
        }

        public bool UpdateArticle(ArticleData article)
        {
            return ArticleRepository.UpdateArticle(article);
        }

        public bool DeleteArticle(ArticleData article)
        {
            return ArticleRepository.DeleteArticle(article);
        }
    }
}
