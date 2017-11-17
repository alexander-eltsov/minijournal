using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Infotecs.MiniJournal.Specs
{
    public class MiniJournalDatabase
    {
        public void Execute(params string[] scriptNames)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string scriptName in scriptNames)
            {
                var scriptBody = GetEmbeddedScript(scriptName);
                if (!string.IsNullOrWhiteSpace(scriptBody))
                {
                    sb.AppendLine(scriptBody);
                }
                else
                {
                    scriptBody = GetDatabaseFolderScript(scriptName);
                    if (!string.IsNullOrWhiteSpace(scriptBody))
                    {
                        sb.AppendLine(scriptBody);
                    }
                    else
                        throw new IOException($"Script {scriptName} not found");
                }
            }
            ExecuteSqlNonQuery(sb.ToString());
        }

        private void ExecuteSqlNonQuery(string sql)
        {
            var connectionString = GetConnectionString("master");
            using (var connection = new SqlConnection(connectionString))
            {
                Server db = new Server(new ServerConnection(connection));
                db.ConnectionContext.ExecuteNonQuery(sql);
            }
        }

        private string GetEmbeddedScript(string scriptName)
        {
            string resource = string.Empty;

            string namespacePart = "Infotecs.MiniJournal.Specs.Resources";
            string resourceFullName = namespacePart + "." + scriptName;

            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName))
            {
                if (stm != null)
                {
                    resource = new StreamReader(stm).ReadToEnd();
                }
            }

            return resource;
        }

        private string GetDatabaseFolderScript(string scriptName)
        {
            string fileName = Path.Combine(Specs.Properties.Settings.Default.DatabaseDir, scriptName);
            return File.ReadAllText(fileName);
        }

        private string GetConnectionString(string initialCatalog = "")
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            if (!string.IsNullOrEmpty(initialCatalog))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.InitialCatalog = "master";
                connectionString = builder.ConnectionString;
            }
            return connectionString;
        }
    }
}
