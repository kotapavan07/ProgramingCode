using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InService.Util
{
    public sealed class DBUtil
    {
        public static IDbConnection GetOpenConnection()
        {
            IDbConnection connection = null;
            string connStr = ConfigurationManager.ConnectionStrings["LocalDBEntities2"].ConnectionString;
            connection = new SqlConnection(connStr);

            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }
    }
}