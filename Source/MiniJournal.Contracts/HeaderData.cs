using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class HeaderData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Caption { get; set; }
    }
}