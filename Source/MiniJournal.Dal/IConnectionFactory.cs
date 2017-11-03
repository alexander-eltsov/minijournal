using System;
using System.Data;

namespace Infotecs.MiniJournal.Dal
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}