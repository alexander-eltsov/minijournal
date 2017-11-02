using System;
using System.Data;
using System.Data.SqlClient;

namespace Infotecs.MiniJournal.Dal
{
    public class ConnectionFactory
    {
        public IDbConnection Create()
        {
            var connectionString = "Data Source=MSK-W0009\\SQLEXPRESS;Initial Catalog=MiniJournalDB;Integrated Security=True";
            var sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }
    }
}
