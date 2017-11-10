﻿using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class RemoveCommentRequest
    {
        [DataMember]
        public int ArticleId { get; set; }

        [DataMember]
        public int CommentId { get; set; }
    }
}
