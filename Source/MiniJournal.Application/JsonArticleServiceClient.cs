using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;
using Nelibur.ServiceModel.Clients;

namespace Infotecs.MiniJournal.Application
{
    public class JsonArticleServiceClient : IArticleService
    {
        private readonly JsonServiceClient serviceClient;

        public JsonArticleServiceClient(JsonServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public IEnumerable<HeaderData> GetArticleHeaders()
        {
            var request = new GetArticleHeadersRequest();
            var response = serviceClient.Get<GetArticleHeadersResponse>(request);

            return response.Headers;
        }

        public ArticleData GetArticle(int articleId)
        {
            var request = new GetArticleRequest
            {
                ArticleId = articleId
            };
            var response = serviceClient.Get<GetArticleResponse>(request);

            return response.Article;
        }

        public void CreateArticle(ArticleData article)
        {
            var request = new CreateArticleRequest
            {
                NewArticle = article
            };
            var response = serviceClient.Post<CreateArticleResponse>(request);
        }

        public void UpdateArticle(ArticleData article)
        {
            var request = new UpdateArticleRequest
            {
                Article = article
            };
            var response = serviceClient.Put<UpdateArticleResponse>(request);
        }

        public void DeleteArticle(int articleId)
        {
            var request = new DeleteArticleRequest
            {
                ArticleId = articleId
            };
            serviceClient.Delete(request);
        }

        public void AddComment(int articleId, CommentData comment)
        {
            var request = new AddCommentRequest
            {
                ArticleId = articleId,
                Comment = comment
            };
            var response = serviceClient.Post<AddCommentResponse>(request);
        }

        public void RemoveComment(int articleId, int commentId)
        {
            var request = new RemoveCommentRequest()
            {
                ArticleId = articleId,
                CommentId = commentId
            };
            serviceClient.Delete(request);
        }
    }
}
