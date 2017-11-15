using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts.Notification
{
    [DataContract]
    public class ArticleMessage : NotificationMessage
    {
        [DataMember]
        public int ArticleId { get; set; }
    }
}
