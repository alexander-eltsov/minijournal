using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class UpdateArticleRequest
    {
        [DataMember]
        public ArticleData Article { get; set; }
    }
}
