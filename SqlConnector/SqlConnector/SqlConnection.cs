using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnector
{
    public class SQLConnector
    {
        string connectionString = null;
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        SqlConnection connection;

        public SQLConnector(string database)
        {
            connectionString = $"Data Source=(local)\\SQLEXPRESS;Initial Catalog={database};User ID=sa;Password=sa123";

        }
        public SQLConnector(string server, string database, string username, string password)
        {
            connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password}";
        }
        /// <summary>
        /// Initializes the connection object. Returns True if connection string was not empty or null.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean InitializeConnection()
        {
            if (connectionString != "" && connectionString != null)
            {
                connection = new SqlConnection(connectionString);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Open(bool capture)
        {
            if (capture)
            {
                return Open();
            } else
            {
                connection.Open();
                return false;
            }
        }
        /// <summary>
        /// Attempts to open the SQL connection. Returns true upon success.
        /// </summary>
        /// <returns></returns>
        public Boolean Open()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return false;
            }
        }
        /// <summary>
        /// Attempts to close the SQL Connection. Returns true upon success.
        /// </summary>
        /// <returns></returns>
        public Boolean Close()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return false;
            }
        }
        public Boolean Close(bool capture)
        {
            if (capture)
            {
                return Close();
            }
            else
            {
                connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Returns a SQLCommand object using the passed query string. 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string queryString)
        {
            if (queryString == null)
            {
                throw new NullQueryStringException("The Query String was null.");
            }

            SqlCommand command = null;
            command = new SqlCommand(queryString, connection);
            return command;
        }

        // TODO: Use DataTable to store SQL results.
        public SqlDataReader ReadResults(SqlCommand sqlCommand)
        {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                return reader;
        }
        public ConnectionState GetConnectionState()
        {
            return connection.State;
        }
    }
    public class NullQueryStringException : Exception
    {
       public NullQueryStringException()
        {
            
        }
        public NullQueryStringException(string message)
    : base(message)
        {
            
        }
        public NullQueryStringException(string message, Exception inner)
    : base(message, inner)
        {
        }
    }

}
