using System;

namespace Infotecs.MiniJournal.Application
{
    public interface ILogger
    {
        void LogEnvironmentInfo();
        void LogError(string error);
        void LogError(Exception exception);
    }
}