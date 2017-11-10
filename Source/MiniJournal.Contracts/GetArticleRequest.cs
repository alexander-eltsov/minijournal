using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class GetArticleRequest
    {
        [DataMember]
        public int ArticleId { get; set; }
    }
}
