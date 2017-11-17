using System;
using TechTalk.SpecFlow;

namespace Infotecs.MiniJournal.Specs
{
    [Binding]
    public sealed class Hooks
    {
        private static readonly MiniJournalDatabase testDB;
        private static readonly MiniJournalServer server;

        static Hooks()
        {
            testDB = new MiniJournalDatabase();
            server = new MiniJournalServer();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            server.StartServiceHost();
            testDB.Execute(
                "create_test_database.sql",
                "use_test_database.sql",
                "init_schema.sql");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            testDB.Execute(
                "init_test_data.sql");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            testDB.Execute(
                "clear_test_data.sql");
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            testDB.Execute(
                "drop_test_database.sql");
            server.StopServiceHost();
        }
    }
}
