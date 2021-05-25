using System;
using System.Net;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public static SqlConnectorInfo.SqlConnectionInfoData SqlInfoData; // Globally accessible instance of the XML loaded SQL Connection Info.
        private static string xmlPath = "SqlConnectorData.xml";
        public Dashboard()
        {
            GetExecutionDirectory();
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            LoadSqlConnectionXml();
        }

        public void GetExecutionDirectory()
        {
            //gets the location the exe and stores it in string.
            string str = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            
            //remove "file:\" from beginning
            xmlPath = str.Substring(6);
            
            //add XML file
            xmlPath += "\\SqlConnectorData.xml";
        }
        public static void LoadSqlConnectionXml()
        {
            SqlInfoData = SqlConnectorInfo.LoadSqlConnectionData(xmlPath);
        }
        private void databaseBackupToolBtn_Click(object sender, EventArgs e)
        {
            LoadSqlConnectionXml();
            Backup backup = new Backup();
            backup.ShowDialog();
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            LoadSqlConnectionXml();
            Restore restore = new Restore();
            restore.ShowDialog();
        }
    }
}
