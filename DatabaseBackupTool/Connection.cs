using SqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Connection : Form
    {
        SQLConnector conn;
        public Connection()
        {
            InitializeComponent();
            serverNameTextBox.Text = "SQLEXPRESS";
            dataSourceTextBox.Text = "(local)";
            initialCatalogTextBox.Text = "";
            userNameTextBox.Text = "sa";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = $"Data Source={dataSourceTextBox.Text}\\{serverNameTextBox.Text};Initial Catalog={initialCatalogTextBox.Text};User ID={userNameTextBox.Text};Password={passwordTextBox.Text};";

        }
    }
}
