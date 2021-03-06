﻿using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using FluentValidation;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Contracts.Notification;
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
        private readonly INotificationService notificationService;
        private readonly AbstractValidator<Article> articleValidator;

        public ArticleProcessor(
            IArticleRepository articleRepository,
            IMapper mapper,
            INotificationService notificationService)
        {
            this.articleRepository = articleRepository;
            this.mapper = mapper;
            this.notificationService = notificationService;
            articleValidator = new ArticleValidator()
                .ChainValidator(new ArticleIsUniqueValidator(articleRepository));
        }

        public object Get(GetArticleRequest request)
        {
            var article = articleRepository
                .GetArticle(request.ArticleId)
                .Map(articleModel => mapper.Map<Article, ArticleData>(articleModel))
                .Some(a => a)
                .None(() => throw new WebFaultException(HttpStatusCode.NotFound));

            return new GetArticleResponse
            {
                Article = article
            };
        }

        public object Post(CreateArticleRequest request)
        {
            var articleModel = mapper.Map<ArticleData, Article>(request.NewArticle);

            var validationResult = articleValidator.Validate(articleModel);
            if (!validationResult.IsValid)
            {
                return new CreateArticleResponse
                {
                    Error = validationResult.Errors.First().ErrorMessage
                };
            }

            articleRepository.CreateArticle(articleModel);

            notificationService.Notify(new ArticleCreatedMessage
            {
                ArticleId = articleModel.Id
            });

            return new CreateArticleResponse
            {
                ArticleId = articleModel.Id
            };
        }

        public object Put(UpdateArticleRequest request)
        {
            var articleModel = mapper.Map<ArticleData, Article>(request.Article);

            var validationResult = articleValidator.Validate(articleModel);
            if (!validationResult.IsValid)
            {
                return new UpdateArticleResponse
                {
                    Error = validationResult.Errors.First().ErrorMessage
                };
            }

            articleRepository.UpdateArticle(articleModel);

            notificationService.Notify(new ArticleUpdatedMessage
            {
                ArticleId = articleModel.Id
            });

            return new UpdateArticleResponse();
        }

        public void DeleteOneWay(DeleteArticleRequest request)
        {
            articleRepository.DeleteArticle(request.ArticleId);

            notificationService.Notify(new ArticleDeletedMessage
            {
                ArticleId = request.ArticleId
            });
        }
    }
}
