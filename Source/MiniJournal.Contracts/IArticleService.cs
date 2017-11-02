using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Infotecs.MiniJournal.Contracts
{
    //[ServiceContract(Name = "ArticleService", Namespace = "http://Infotecs.MiniJournal.Service")]
    [ServiceContract]
    public interface IArticleService
    {
        [OperationContract]
        IEnumerable<ArticleData> GetAllArticles();
    }
}
