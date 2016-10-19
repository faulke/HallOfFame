using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace WebApplication1
{
    public static class DataConnection
    {
        public const string ConnectionStringName = "DefaultConnection";

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            connection.Open();
            return connection;
        }

    }
}