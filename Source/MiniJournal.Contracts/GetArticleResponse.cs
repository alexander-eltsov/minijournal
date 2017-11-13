﻿using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class GetArticleResponse
    {
        [DataMember]
        public ArticleData Article { get; set; }


        //[DataMember]
        //public bool HasError { get; set; }

        [DataMember]
        public Exception Error { get; set; }

    }
}
