using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data;
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
            }
            catch (Exception e)
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
                ServerInstanceComboBox_SelectionChangeCommitted(sender, e);
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
                ServerInstanceComboBox_SelectionChangeCommitted(sender, e);
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
            Logger.Error(e.Message);
            ef.ControlBox = false;
            ef.ShowDialog();
        }

        private void LoadSqlServerInstancesAsync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DataTable table = SmoApplication.EnumAvailableSqlServers(true);
            LoadSqlServerInstancesAsync.ReportProgress(100, table);
        }

        private void LoadSqlServerInstancesAsync_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            DataTable table = (DataTable)e.UserState;
            DataRow addDefault = table.NewRow();
            string defaultInstance = ServerInstanceComboBox.SelectedItem.ToString();
            ServerInstanceComboBox.ValueMember = "Name";
            ServerInstanceComboBox.DataSource = table;

            addDefault["Name"] = defaultInstance;
            addDefault["Server"] = "(local)";
            addDefault["Instance"] = "SQLEXPRESS";
            addDefault["IsClustered"] = "false";
            addDefault["Version"] = "11.0.6020.0";
            addDefault["IsLocal"] = "true";
            table.Rows.InsertAt(addDefault, 0);
            ServerInstanceComboBox.SelectedIndex = 0;

            ServerInstanceComboBox.Enabled = true;
            Logger.Info("Loading complete!");
        }

        private void ServerInstanceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                int i = ServerInstanceComboBox.SelectedIndex;
                DataTable data = (DataTable)ServerInstanceComboBox.DataSource;
                DataRow row = data.Rows[i];
                SqlInfoData.Data_Source = row["Name"].ToString();
            }
            catch { }
        }
    }
}
