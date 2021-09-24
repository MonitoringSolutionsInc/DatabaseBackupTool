using System;
using System.Net;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public static SqlConnectorInfo.SqlConnectionInfoData SqlInfoData; // Globally accessible instance of the XML loaded SQL Connection Info.
        private static string xmlPath = "SqlConnectorData.xml";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
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
            try
            {
                SqlInfoData = SqlConnectorInfo.LoadSqlConnectionData(xmlPath);
                Logger.Info($"Successfully loaded SqlConnection information from XML Path: {xmlPath}");
            } catch (Exception e)
            {
                Logger.Error(e, $"An error has occurred while attempting to load SqlConnection information from XML Path: {xmlPath}");
                throw;
            }
        }
        private void databaseBackupToolBtn_Click(object sender, EventArgs e)
        {
            LoadSqlConnectionXml();
            if (HelperClass.CanConnectToSQL())
            {
                Backup backup = new Backup();
                backup.ShowDialog();
            } else
            {
                DisplayConnectionErrorForm();
            }
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            LoadSqlConnectionXml();
            if (HelperClass.CanConnectToSQL())
            {
                Restore restore = new Restore();
                restore.ShowDialog();
            } else
            {
                DisplayConnectionErrorForm();
            }
        }

        private void DisplayConnectionErrorForm()
        {
            Exception e = new Exception("Could not connect to SQL using the configured connection details. Check the configuration file for proper entry.");
            ErrorForm ef = new ErrorForm(e);
            Logger.Error(e);
            ef.ControlBox = false;

            ef.ShowDialog();
        }
    }
}
