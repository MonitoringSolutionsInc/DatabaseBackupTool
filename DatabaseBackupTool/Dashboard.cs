using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data;
using System.Net;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public static SqlConnectorInfo.SqlConnectionInfoData SqlInfoData; // Globally accessible instance of the XML loaded SQL Connection Info.
        private static string xmlPath = "SqlConnectorData.xml";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
        public Dashboard()
        { 
            InitializeComponent();
            LoadSqlServerInstancesAsync.RunWorkerAsync();
            GetExecutionDirectory();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            LoadSqlConnectionXml();
            ServerInstanceComboBox.Enabled = false;
            ServerInstanceComboBox.Items.Add(SqlInfoData.Data_Source);
            ServerInstanceComboBox.SelectedIndex = 0;
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
            Backup backup = new Backup();
            backup.ShowDialog();
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            LoadSqlConnectionXml();
            Restore restore = new Restore();
            restore.ShowDialog();
        }

        private void LoadSqlServerInstancesAsync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DataTable table = instance.GetDataSources();
            LoadSqlServerInstancesAsync.ReportProgress(100, table);
        }

        private void LoadSqlServerInstancesAsync_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            DataTable table = (DataTable)e.UserState;
            ServerInstanceComboBox.ValueMember = "InstanceName";
            ServerInstanceComboBox.DataSource = table;
            ServerInstanceComboBox.SelectedIndex = 0;
            ServerInstanceComboBox.Enabled = true;
            Logger.Info("Loading complete!");
        }

        private void ServerInstanceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int i = ServerInstanceComboBox.SelectedIndex;
            DataTable data = (DataTable)ServerInstanceComboBox.DataSource;
            DataRow row = data.Rows[i];
            SqlInfoData.Data_Source = $"{row["ServerName"]}\\{row["InstanceName"]}";
        }
    }
}
