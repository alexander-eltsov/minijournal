using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class CommentData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string User { get; set; }
    }
}
