using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;

namespace Infotecs.MiniJournal.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;

        public ArticleService(
            IArticleRepository articleRepository,
            IMapper mapper)
        {
            if (articleRepository == null)
            {
                throw new ArgumentNullException(nameof(articleRepository));
            }

            this.articleRepository = articleRepository;
            this.mapper = mapper;
        }

        public IEnumerable<ArticleData> GetAllArticles()
        {
            IList<Article> articles = articleRepository.GetArticles();
            IEnumerable<ArticleData> articleDatas = mapper.Map<IList<Article>, IEnumerable<ArticleData>>(articles);

            return articleDatas;
        }

        public void CreateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            articleRepository.CreateArticle(articleModel);
        }

        public void UpdateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            articleRepository.UpdateArticle(articleModel);
        }

        public void DeleteArticle(int articleId)
        {
            articleRepository.DeleteArticle(articleId);
        }
    }
}
