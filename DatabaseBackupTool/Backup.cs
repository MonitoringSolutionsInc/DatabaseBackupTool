using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlConnector; 

namespace DatabaseBackupTool
{
    public partial class Backup : Form
    {
        ErrorForm ef;
        SQLConnector connector;
        public Backup()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
        }

        public void InitializeConnection()
        {
            connector = new SQLConnector("");
            connector.InitializeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Connect to (local)\SQLEXPRESS
            // Find list of all database names 
            InitializeConnection();
            foreach(String s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Closing"); 
        }

        public List<String> GetDatabases()
        {
            List<String> databases = new List<String>();
            try
            {
                if (connector.Open())
                {
                    var reader = connector.ReadResults(connector.CreateCommand("SELECT name FROM master.sys.databases where name NOT IN ('master','model','msdb','tempdb')"));
                    while (reader.Read())
                    {
                        databases.Add(reader[0].ToString());
                    }
                    reader.Close();
                    if (connector.GetConnectionState() == ConnectionState.Open)
                    {
                        connector.Close();
                    }
                }
                return databases;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}: There was an error while retrieving the database list.");
                return databases;
            }
        }
        
        public void MoveSelectedItems(string direction, bool all=false)
        {

            List<String> movedList = new List<String>();
            if (direction == "right")
            {
                if (all) // If moving all items to the right.
                {
                    for (int entry = 0; entry < databaseList.Items.Count; entry++)
                    {
                        databaseList.SetSelected(entry, true);
                    }
                }
                foreach(String s in databaseList.SelectedItems)
                {
                    if (!backupList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach(String s in movedList)
                {
                    backupList.Items.Add(s);
                    if (databaseList.Items.Contains(s)){
                        databaseList.Items.Remove(s);
                    }
                }
                
            } 
            else if (direction == "left")
            {
                if(all) // If moving all items left.
                {
                    for (int entry = 0; entry < backupList.Items.Count; entry++)
                    {
                        backupList.SetSelected(entry, true);
                    }
                }
                foreach(String s in backupList.SelectedItems)
                {
                    if (!databaseList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach(String s in movedList)
                {
                    databaseList.Items.Add(s);
                    if (backupList.Items.Contains(s))
                    {
                        backupList.Items.Remove(s);
                    }
                }
            }
        }
        private void MoveSelectRight_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("right");
        }

        private void MoveSelectLeft_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("left");
        }

        private void MoveAllRight_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("right", true);
        }

        private void MoveAllLeft_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("left", true);
        }

        private void RefreshDBs_Click(object sender, EventArgs e)
        {
            databaseList.Items.Clear();
            backupList.Items.Clear();

            foreach(String s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }
        }

        private void startBackUp_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                backupProgressBar.Value = 0;
                startBackUp.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            backupDirectoryTextBox.Text = fbd.SelectedPath;
        }

        private void databaseList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void backupDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            if(backupDirectoryTextBox.Text == "")
            {
                backupDirectoryTextBox.Text = "C:\\CEMDAS\\temp";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for(int i = 0; i < backupList.Items.Count; i++)
            {
                if (connector.Open())
                {
                    try
                    {
                        string sql = $"BACKUP DATABASE \"{backupList.Items[i]}\" TO DISK = \'{backupDirectoryTextBox.Text}\\{backupList.Items[i]}.BAK\' WITH INIT";
                        // string sql = $"BACKUP DATABASE \"{s}\" TO DISK = \'{backupDirectoryTextBox.Text}\\{s}.BAK\'";
                        var command = connector.CreateCommand(sql);
                        command.CommandTimeout = 0;
                        var reader = connector.ReadResults(command);
                        if (connector.GetConnectionState() == ConnectionState.Open)
                        {
                            connector.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        connector.Close();
                        ef = new ErrorForm(ex);
                        ef.Show();
                        break;
                    }
                }
                if (backupProgressBar.Value == backupProgressBar.Maximum)
                {
                    MessageBox.Show($"Backup Complete! You backed up {backupList.Items.Count} files!", "Backup Complete", MessageBoxButtons.OK);
                }
                int percentComplete =
                                    (int)((float)i / (float)(backupList.Items.Count) * 100);
                Console.WriteLine($"{percentComplete}% Finished");
                worker.ReportProgress(percentComplete);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            backupProgressBar.Value = e.ProgressPercentage;
            progressBarLabel.Text = $"{e.ProgressPercentage}% Complete";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBackUp.Enabled = true;
            backupProgressBar.Value = 100;
            progressBarLabel.Text = $"100% Complete";
        }
    }
}
