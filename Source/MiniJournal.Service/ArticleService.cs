using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;

namespace Infotecs.MiniJournal.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;
        private readonly AbstractValidator<Article> articleValidator;
        private readonly AbstractValidator<Comment> commentValidator;

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

            articleValidator = new ArticleValidator()
                .ChainValidator(new ArticleIsUniqueValidator(articleRepository));
            commentValidator = new CommentValidator();
        }

        public IEnumerable<HeaderData> GetArticleHeaders()
        {
            IList<Header> headers = articleRepository.GetHeaders();
            IEnumerable<HeaderData> headerDatas = mapper.Map<IList<Header>, IEnumerable<HeaderData>>(headers);

            return headerDatas;
        }

        public ArticleData GetArticle(int articleId)
        {
            var article = articleRepository.GetArticle(articleId);
            var articleData = mapper.Map<Article, ArticleData>(article);

            return articleData;
        }

        public void CreateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            ValidateArticle(articleModel);
            articleRepository.CreateArticle(articleModel);
        }

        public void UpdateArticle(ArticleData article)
        {
            var articleModel = mapper.Map<ArticleData, Article>(article);
            ValidateArticle(articleModel);
            articleRepository.UpdateArticle(articleModel, updateComments: false);
        }

        public void DeleteArticle(int articleId)
        {
            articleRepository.DeleteArticle(articleId);
        }

        public void AddComment(int articleId, CommentData comment)
        {
            var article = articleRepository.GetArticle(articleId);
            var commentModel = mapper.Map<CommentData, Comment>(comment);

            ValidateComment(commentModel);
            article.AddComment(commentModel);

            articleRepository.CreateArticleComment(commentModel);
        }

        public void RemoveComment(int articleId, int commentId)
        {
            articleRepository.DeleteArticleComment(articleId, commentId);
        }

        protected virtual void ValidateArticle(Article article)
        {
            var validationResult = articleValidator.Validate(article);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.First().ErrorMessage);
            }
        }

        protected virtual void ValidateComment(Comment comment)
        {
            var validationResult = commentValidator.Validate(comment);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.First().ErrorMessage);
            }
        }
    }
}
