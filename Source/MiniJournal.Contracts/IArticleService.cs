using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Infotecs.MiniJournal.Contracts
{
    [ServiceContract]
    public interface IArticleService
    {
        [OperationContract]
        IEnumerable<ArticleData> GetAllArticles();

        [OperationContract]
        bool CreateArticle(ArticleData article);

        [OperationContract]
        bool UpdateArticle(ArticleData article);

        [OperationContract]
        bool DeleteArticle(ArticleData article);
    }
}
