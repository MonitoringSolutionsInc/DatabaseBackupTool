using SqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackupTool
{
    public static class HelperClass
    {
        public static bool ContainsBakFiles(String path, bool recursiveChecked)
        {
            SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveChecked);
            string[] filesToRestore = Directory.GetFiles(path, "*.bak", recursive);
            if (filesToRestore.Length <= 0)
            {
                return false;
            }
            return true;
        }

        public static bool SqlServerHasReadAccess(String path)
        {
            string databaseName = "deleteMe";
            path += $"{databaseName}.BAK";
            string restoreDatabase = $@"RESTORE DATABASE [{databaseName}] FROM DISK='{path}' WITH REPLACE";
            string destroyDatabase = $@"DROP DATABASE [{databaseName}]";
            SQLConnector conn = null;
            try
            {
                using (File.Create(path)) { }
                conn = connectToSQL();

                conn.Open();
                var command = conn.CreateCommand(restoreDatabase);
                command.CommandTimeout = 0;
                conn.ReadResults(command);
                conn.Close();

                conn.Open();
                command = conn.CreateCommand(destroyDatabase);
                conn.ReadResults(command);
                conn.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("Access is denied"))
                {
                    return false;
                }
                else
                    return true;
            }
            finally
            {
                if (conn != null)
                {
                    File.Delete(path);
                    conn.Dispose();
                }
            }

            return true;
        }

        public static SQLConnector connectToSQL()
        {
            SQLConnector conn = null;
            try
            {
                conn = new SQLConnector(Dashboard.SqlInfoData.Data_Source, Dashboard.SqlInfoData.Initial_Catalog, Dashboard.SqlInfoData.User_Id, Dashboard.SqlInfoData.Password);
                conn.InitializeConnection();
            }
            catch (Exception ex)
            {
                if (conn != null && conn.GetConnectionState() == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return conn;
        }

        public static bool CanConnectToSQL()
        {
            SQLConnector connector = new SQLConnector(Dashboard.SqlInfoData.Data_Source, Dashboard.SqlInfoData.Initial_Catalog, Dashboard.SqlInfoData.User_Id, Dashboard.SqlInfoData.Password);
            connector.InitializeConnection();
            bool canBeOpened = connector.Open();
            connector.Close();
            return canBeOpened;
        }
    }
}
