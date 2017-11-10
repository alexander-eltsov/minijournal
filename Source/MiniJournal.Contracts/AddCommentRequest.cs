using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class AddCommentRequest
    {
        [DataMember]
        public int ArticleId { get; set; }

        [DataMember]
        public CommentData Comment { get; set; }
    }
}
