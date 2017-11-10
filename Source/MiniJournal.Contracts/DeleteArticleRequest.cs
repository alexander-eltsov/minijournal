using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class DeleteArticleRequest
    {
        [DataMember]
        public int ArticleId { get; set; }
    }
}
