using System;
using System.Data;
using System.Data.SqlClient;

namespace Infotecs.MiniJournal.Dal
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public ConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            var sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}
