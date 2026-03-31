using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Festival.Repository
{
    public static class DbUtils
    {
        private static IDbConnection _instance = null;

        public static IDbConnection GetConnection()
        {
            if (_instance == null || _instance.State == ConnectionState.Closed)
            {
                string connString = ConfigurationManager.ConnectionStrings["festival.db"].ConnectionString;
                _instance = new SQLiteConnection(connString);
                _instance.Open();
            }
            return _instance;
        }
    }
}