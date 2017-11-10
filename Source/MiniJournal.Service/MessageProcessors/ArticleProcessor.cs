using System;
using System.Linq;
using FluentValidation;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;
using Nelibur.ServiceModel.Services.Operations;

namespace Infotecs.MiniJournal.Service.MessageProcessors
{
    public class ArticleProcessor :
        IGet<GetArticleRequest>,
        IPost<CreateArticleRequest>,
        IPut<UpdateArticleRequest>,
        IDeleteOneWay<DeleteArticleRequest>
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;
        private readonly AbstractValidator<Article> articleValidator;

        public ArticleProcessor(
            IArticleRepository articleRepository,
            IMapper mapper)
        {
            this.articleRepository = articleRepository;
            this.mapper = mapper;
            articleValidator = new ArticleValidator()
                .ChainValidator(new ArticleIsUniqueValidator(articleRepository));
        }

        public object Get(GetArticleRequest request)
        {
            var article = articleRepository.GetArticle(request.ArticleId);
            var articleData = mapper.Map<Article, ArticleData>(article);

            return new GetArticleResponse
            {
                Article = articleData
            };
        }

        public object Post(CreateArticleRequest request)
        {
            var articleModel = mapper.Map<ArticleData, Article>(request.NewArticle);
            ValidateArticle(articleModel);
            articleRepository.CreateArticle(articleModel);

            return new CreateArticleResponse();
        }

        public object Put(UpdateArticleRequest request)
        {
            var articleModel = mapper.Map<ArticleData, Article>(request.Article);
            ValidateArticle(articleModel);
            articleRepository.UpdateArticle(articleModel);

            return new UpdateArticleResponse();
        }

        public void DeleteOneWay(DeleteArticleRequest request)
        {
            articleRepository.DeleteArticle(request.ArticleId);
        }

        protected virtual void ValidateArticle(Article article)
        {
            var validationResult = articleValidator.Validate(article);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.First().ErrorMessage);
            }
        }
    }
}
