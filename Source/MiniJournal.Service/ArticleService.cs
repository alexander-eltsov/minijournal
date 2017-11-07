using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;

namespace Infotecs.MiniJournal.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;
        private readonly AbstractValidator<Article> articleValidator;

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
            this.articleValidator = new ArticleValidator()
                .ChainValidator(new ArticleIsUniqueValidator(articleRepository));
        }

        public IEnumerable<HeaderData> GetArticleHeaders()
        {
            IList<Article> articles = articleRepository.GetArticles();
            IEnumerable<HeaderData> headerDatas = mapper.Map<IList<Article>, IEnumerable<HeaderData>>(articles);

            return headerDatas;
        }

        public ArticleData LoadArticle(int articleId)
        {
            var article = articleRepository.FindArticle(articleId);
            var articleData = mapper.Map<Article, ArticleData>(article);

            return articleData;
        }

        public void CreateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            var validationResult = articleValidator.Validate(articleModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            articleRepository.CreateArticle(articleModel);
        }

        public void UpdateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            var validationResult = articleValidator.Validate(articleModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.Single().ErrorMessage);
            }

            articleRepository.UpdateArticle(articleModel);
        }

        public void DeleteArticle(int articleId)
        {
            articleRepository.DeleteArticle(articleId);
        }
    }
}
