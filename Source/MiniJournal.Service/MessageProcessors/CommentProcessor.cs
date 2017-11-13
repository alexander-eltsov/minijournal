using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using FluentValidation;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Infotecs.MiniJournal.Service.Validators;
using Nelibur.ServiceModel.Services.Operations;

namespace Infotecs.MiniJournal.Service.MessageProcessors
{
    public sealed class CommentProcessor :
        IPost<AddCommentRequest>,
        IDeleteOneWay<RemoveCommentRequest>
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;
        private readonly AbstractValidator<Comment> commentValidator;

        public CommentProcessor(
            IArticleRepository articleRepository,
            IMapper mapper)
        {
            this.articleRepository = articleRepository;
            this.mapper = mapper;
            commentValidator = new CommentValidator();
        }

        public object Post(AddCommentRequest request)
        {
            var article = articleRepository
                .GetArticle(request.ArticleId)
                .Some(a => a)
                .None(() => throw new WebFaultException(HttpStatusCode.NotFound));
            var comment = mapper
                .Map<CommentData, Comment>(request.Comment);

            ValidateComment(comment);
            article.AddComment(comment);
            articleRepository.CreateArticleComment(comment);

            return new AddCommentResponse
            {
                ArticleId = article.Id,
                CommentId = comment.Id
            };
        }

        public void DeleteOneWay(RemoveCommentRequest request)
        {
            articleRepository.DeleteArticleComment(request.ArticleId, request.CommentId);
        }

        private void ValidateComment(Comment comment)
        {
            var validationResult = commentValidator.Validate(comment);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.First().ErrorMessage);
            }
        }
    }
}
