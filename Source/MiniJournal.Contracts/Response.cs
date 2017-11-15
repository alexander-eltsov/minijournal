using System;
using System.Net;
using System.Runtime.Serialization;

namespace Infotecs.MiniJournal.Contracts
{
    [DataContract]
    public class Response
    {
        public Response()
        {
            StatusCode = HttpStatusCode.OK;
        }

        [DataMember]
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string Error { get; set; }

        public bool HasError => !string.IsNullOrEmpty(Error);
    }
}
