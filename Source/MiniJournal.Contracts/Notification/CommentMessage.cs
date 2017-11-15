using System;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts.Notification
{
    [DataContract]
    public class CommentMessage : NotificationMessage
    {
        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public int CommmentId { get; set; }
    }
}
