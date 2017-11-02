using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class ArticleData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}
