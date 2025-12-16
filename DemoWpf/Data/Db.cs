using System.Configuration;
using System.Data.SqlClient;


namespace DemoWpf.Data
{
    internal class Db
    {
        private static readonly string _connectionString = 
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
