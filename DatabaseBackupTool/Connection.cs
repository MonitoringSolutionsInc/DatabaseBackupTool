using SqlConnector;
using System;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Connection : Form
    {
        public static SQLConnector PrimaryConnection;
        public static string DataSource = "";
        public Connection()
        {
            InitializeComponent();
            serverNameTextBox.Text = "SQLEXPRESS";
            dataSourceTextBox.Text = "(local)";
            initialCatalogTextBox.Text = "";
            userNameTextBox.Text = "sa";
            passwordTextBox.Text = "sa123";
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            PrimaryConnection = new SQLConnector("");
            string connectionString = $"Data Source={dataSourceTextBox.Text}\\{serverNameTextBox.Text};Initial Catalog={initialCatalogTextBox.Text};User ID={userNameTextBox.Text};Password={passwordTextBox.Text};";
            PrimaryConnection.ConnectionString = connectionString;
            PrimaryConnection.InitializeConnection();
            try
            {
                if (PrimaryConnection.Open() == false || PrimaryConnection.Close() == false)
                {
                    throw new ConnectionInvalidException($"Could not connect to \"{dataSourceTextBox.Text}\"");
                } else
                {
                    DataSource = dataSourceTextBox.Text;
                    Dashboard d_form = new Dashboard();
                    d_form.ShowDialog();
                }

            } catch (Exception ex)
            {
                ErrorForm e_form = new ErrorForm(ex);
                e_form.ShowDialog();
            }
        }
    }
    public class ConnectionInvalidException : Exception
    {
        public ConnectionInvalidException(string message): base(message)
        {

        }
    }
}