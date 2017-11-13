using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class AddCommentResponse
    {
        [DataMember]
        public int ArticleId { get; set; }

        [DataMember]
        public int CommentId { get; set; }
    }
}
