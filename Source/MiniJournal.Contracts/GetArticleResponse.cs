using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class GetArticleResponse : Response
    {
        [DataMember]
        public ArticleData Article { get; set; }
    }
}
