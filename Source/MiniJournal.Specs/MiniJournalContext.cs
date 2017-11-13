using System;
using Nelibur.ServiceModel.Clients;

namespace Infotecs.MiniJournal.Specs
{
    public class MiniJournalContext
    {
        public MiniJournalContext()
        {
            ServiceClient = new JsonServiceClient("http://localhost:8082/article");
        }

        public JsonServiceClient ServiceClient { get; private set; }
    }
}
