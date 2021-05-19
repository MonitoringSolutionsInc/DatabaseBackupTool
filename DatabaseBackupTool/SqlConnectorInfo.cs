using System;
using System.Linq;
using System.Xml.Linq;

namespace DatabaseBackupTool
{
    public abstract class SqlConnectorInfo  // Abstract, we do not need instances of this class.
    {
        public struct SqlConnectionInfoData
        {
            public string Data_Source;
            public string Initial_Catalog;
            public string User_Id;
            public string Password;
        }
        /// <summary>
        /// Loads the Connection String data from the given XML path.
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns>SqlConnectionInfoData</returns>
        public static SqlConnectionInfoData LoadSqlConnectionData(string xmlPath)
        {
            SqlConnectionInfoData sqlInfo = new SqlConnectionInfoData();
            XElement doc = null;
            try
            {
                doc = XElement.Load(xmlPath);
            } catch (Exception e)
            {
                Console.WriteLine("An error occured while attempting to load Connection String from XML");
            }
            var element = doc.Elements().ToList()[0];

            sqlInfo.Data_Source = element.Attribute("data_source").Value;
            sqlInfo.Initial_Catalog = element.Attribute("initial_catalog").Value;
            sqlInfo.User_Id = element.Attribute("user_id").Value;
            sqlInfo.Password = element.Attribute("password").Value;

            return sqlInfo;
        }
    }
}
