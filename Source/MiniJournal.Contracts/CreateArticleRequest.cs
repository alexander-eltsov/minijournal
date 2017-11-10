using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class CreateArticleRequest
    {
        [DataMember]
        public ArticleData NewArticle { get; set; }
    }
}
