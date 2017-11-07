using System;
using Infotecs.Opus;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Infotecs.MiniJournal.Application
{
    public class Logger : ILogger
    {
        private readonly NLog.Logger logger;

        public Logger()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            fileTarget.FileName = @"${basedir}/logs/error.log";
            fileTarget.Layout = @"[${date:format=HH\:mm\:ss.fff}] ${message}";

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule1);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            logger = LogManager.GetLogger("Logger");
        }

        public void LogEnvironmentInfo()
        {
            var dumper = OpusDumper.Configure(x =>
            {
                x.Include(InformationKind.ApplicationInfo);
                x.Include(InformationKind.NetFrameworkInstalled);
                x.Include(InformationKind.OperatingSystem);
                x.Include(InformationKind.ViPNetCSP);
            });
            string result = dumper.Dump();
            logger.Info(result);
        }

        public void LogError(string error)
        {
            logger.Error(error);
        }

        public void LogError(Exception exception)
        {
            logger.Error(exception);
        }
    }
}
