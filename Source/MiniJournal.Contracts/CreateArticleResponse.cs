﻿using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class CreateArticleResponse
    {
        [DataMember]
        public int ArticleId { get; set; }
    }
}