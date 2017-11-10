using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public sealed class GetArticleHeadersResponse
    {
        [DataMember]
        public IEnumerable<HeaderData> Headers { get; set; }
    }
}
