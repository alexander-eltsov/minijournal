using System;
using System.Diagnostics;
using Infotecs.MiniJournal.Specs.Properties;
using Nelibur.ServiceModel.Clients;

namespace Infotecs.MiniJournal.Specs
{
    public class MiniJournalContext
    {
        public MiniJournalContext()
        {
            ServiceClient = new JsonServiceClient(Settings.Default.ServiceAddress);
        }

        public JsonServiceClient ServiceClient { get; private set; }
    }
}
