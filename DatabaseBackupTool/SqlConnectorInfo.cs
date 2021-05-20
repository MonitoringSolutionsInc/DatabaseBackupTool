using System;
using System.IO;
using System.Linq;
using System.Text;
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
                var element = doc.Elements().ToList()[0];
                sqlInfo.Data_Source = element.Attribute("data_source").Value;
                sqlInfo.Initial_Catalog = element.Attribute("initial_catalog").Value;
                sqlInfo.User_Id = element.Attribute("user_id").Value;
                sqlInfo.Password = element.Attribute("password").Value;
            }
            catch (System.IO.FileNotFoundException fnfEx)
            {
                Console.WriteLine($"{fnfEx.Message}\nCreating SqlConnectorData.xml");
                string xmlData = "<? xml version = \"1.0\" encoding = \"utf-8\" ?>< connectionData><connection data_source = \"(local)\\SQLEXPRESS\" initial_catalog =\"\" user_id =\"sa\" password =\"sa123\"></connection></connectionData>";
                Console.WriteLine(xmlData);
                using (FileStream fs = File.OpenWrite("SqlConnectorData.xml"))
                {
                    var info = new UTF8Encoding(true).GetBytes(xmlData);
                    fs.Write(info, 0, info.Length);
                }
                Dashboard.LoadSqlConnectionXml();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while attempting to load Connection String from XML");
                sqlInfo.Data_Source = @"(local)\SQLEXPRESS";
                sqlInfo.Initial_Catalog = "";
                sqlInfo.User_Id = "sa";
                sqlInfo.Password = "sa123";
            }
            return sqlInfo;
        }
    }
}
