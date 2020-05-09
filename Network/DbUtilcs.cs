using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Network
{
    class DbUtilcs
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ExamConnectionString"].ConnectionString;
        }

        public string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
                returnValue = settings.ConnectionString;
            return returnValue;
        }

        public SqlConnection GetSqlConnection(string connectionString)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Dispose();
            }
            return connection;
        }
    }
}
