using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Infotecs.MiniJournal.Contracts
{
    [ServiceContract]
    public interface IArticleService
    {
        [OperationContract]
        IEnumerable<HeaderData> GetArticleHeaders();

        [OperationContract]
        ArticleData LoadArticle(int articleId);

        [OperationContract]
        void CreateArticle(ArticleData article);

        [OperationContract]
        void UpdateArticle(ArticleData article);

        [OperationContract]
        void DeleteArticle(int articleId);
    }
}
