using SqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; 

namespace DatabaseBackupTool
{
    public partial class Restore : Form
    {
        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            restoreDirectoryTextBox.Text = fbd.SelectedPath;
        }

        private void startRestore_Click(object sender, EventArgs e)
        {
            SearchOption recursive = (SearchOption) Convert.ToInt32(recursiveBox.Checked);
            string[] filesToRestore = Directory.GetFiles(restoreDirectoryTextBox.Text, "*.bak", recursive);

            foreach (string fileName in filesToRestore)
            {
                Console.WriteLine(fileName);
                string databaseName = fileName.Split('\\').Last().Split('.').First();
                string restoreSql = $@"RESTORE DATABASE [{databaseName}] FROM DISK='{fileName}' WITH REPLACE";
                ThreadPool.SetMinThreads(1, 0);
                ThreadPool.SetMaxThreads(100, 0);
                new Thread(() =>
                {
                    SQLConnector conn = null;
                    try
                    {
                        conn = new SQLConnector("");
                        conn.InitializeConnection();
                        conn.Open();
                        conn.ReadResults(conn.CreateCommand(restoreSql));
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        if (conn != null && conn.GetConnectionState() == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }).Start(); 
            }
        }
    }
}
