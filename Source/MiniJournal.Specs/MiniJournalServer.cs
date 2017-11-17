using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Infotecs.MiniJournal.Specs
{
    public class MiniJournalServer
    {
        private Process ServerProcess { get; set; }

        public void StartServiceHost()
        {
            var dir = TestContext.CurrentContext.TestDirectory;
            var path = System.IO.Path.Combine(dir, "Infotecs.MiniJournal.Service.exe");
            ServerProcess = new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = path,
                    UseShellExecute = true
                }
            };
            ServerProcess.Start();
        }

        public void StopServiceHost()
        {
            ServerProcess.Kill();
        }
    }
}
