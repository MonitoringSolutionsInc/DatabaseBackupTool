using SqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Backup : Form
    {
        ErrorForm ef;
        SQLConnector connector;
        SQLConnector connector2;
        static public string default_text = "Default Text...";
        private int percentComplete1 = 0;
        private int percentComplete3 = 0;
        private bool backgroundFinished = false;
        public Backup()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker3.WorkerReportsProgress = true;
        }

        public void InitializeConnection()
        {
            connector = new SQLConnector("");
            connector.InitializeConnection();
            connector2 = new SQLConnector("");
            connector2.InitializeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Connect to (local)\SQLEXPRESS
            // Find list of all database names 
            InitializeConnection();
            foreach (string s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }

            // handles the default gray text
            // it going away upon entering the text box
            // and it coming back if textbox is empty upon leaving it
            filterTextBox.Enter += new EventHandler(filterTextBox_Enter);
            filterTextBox.Leave += new EventHandler(filterTextBox_Leave);
            filterTextBox_SetText();
        }

        private void filterTextBox_Enter(object sender, EventArgs e)
        {
            if (filterTextBox.ForeColor == Color.Black)
                return;
            filterTextBox.Text = "";
            filterTextBox.ForeColor = Color.Black;
        }

        private void filterTextBox_Leave(object sender, EventArgs e)
        {
            if (filterTextBox.Text.Trim() == "")
                filterTextBox_SetText();
        }

        private void filterTextBox_SetText()
        {
            filterTextBox.Text = default_text;
            filterTextBox.ForeColor = Color.Gray;
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Closing");
        }

        public List<string> GetDatabases()
        {
            List<string> databases = new List<string>();
            string sqlCommand = "SELECT name FROM master.sys.databases where name NOT IN ('master','model','msdb','tempdb')";
            try
            {
                if (connector.Open())
                {
                    if (filterTextBox.Text != default_text)
                        sqlCommand += " AND name LIKE '%" + filterTextBox.Text + "%'";
                    var reader = connector.ReadResults(connector.CreateCommand(sqlCommand));
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

        public void MoveSelectedItems(string direction, bool all = false)
        {

            List<string> movedList = new List<string>();
            if (direction == "right")
            {
                if (all) // If moving all items to the right.
                {
                    for (int entry = 0; entry < databaseList.Items.Count; entry++)
                    {
                        databaseList.SetSelected(entry, true);
                    }
                }
                foreach (string s in databaseList.SelectedItems)
                {
                    if (!backupList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach (string s in movedList)
                {
                    backupList.Items.Add(s);
                    if (databaseList.Items.Contains(s))
                    {
                        databaseList.Items.Remove(s);
                    }
                }

            }
            else if (direction == "left")
            {
                if (all) // If moving all items left.
                {
                    for (int entry = 0; entry < backupList.Items.Count; entry++)
                    {
                        backupList.SetSelected(entry, true);
                    }
                }
                foreach (string s in backupList.SelectedItems)
                {
                    if (!databaseList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach (string s in movedList)
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

            foreach (string s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }
        }

        private void startBackUp_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backupProgressBar.Value = 0;
                startBackUp.Enabled = false;
                RefreshDBs.Enabled = false;
                MoveSelectRight.Enabled = false;
                MoveSelectLeft.Enabled = false;
                MoveAllLeft.Enabled = false;
                MoveAllRight.Enabled = false;
                backupDirectoryTextBox.Enabled = false;
                filterTextBox.Enabled = false;
                chooseDirectoryButton.Enabled = false;
                try
                {
                    if (!Directory.Exists(backupDirectoryTextBox.Text)) //if directory does not exist
                        Directory.CreateDirectory(backupDirectoryTextBox.Text);
                }
                catch (Exception ex) //could fail if trying to put in a place it doesn't have permission to or make a new drive
                {
                    //show error box
                    string myMessage = ex.Message + "\nThe folowing directory could not be created:\n" + backupDirectoryTextBox.Text;
                    Exception myException = new Exception(myMessage);
                    ef = new ErrorForm(myException);
                    ef.Show();
                }
                backgroundFinished = false;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker3.RunWorkerAsync();
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
            if (backupDirectoryTextBox.Text == "")
            {
                backupDirectoryTextBox.Text = "C:\\CEMDAS\\temp";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 0; i < backupList.Items.Count; i += 2)
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
                //if (backupProgressBar.Value == backupProgressBar.Maximum)
                //{
                //    MessageBox.Show($"Backup Complete! You backed up {backupList.Items.Count} files!", "Backup Complete", MessageBoxButtons.OK);
                //}
                int percentComplete = (int)(i / (float)(backupList.Items.Count) * 100);
                worker.ReportProgress(percentComplete);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete1 = e.ProgressPercentage;
            int done = (percentComplete1 + percentComplete3) / 2;
            backupProgressBar.Value = done;
            Console.WriteLine($"{done}% FinishedBG1");
            progressBarLabel.Text = $"{done}% Complete";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundFinished)
            {
                startBackUp.Enabled = true;
                RefreshDBs.Enabled = true;
                MoveSelectRight.Enabled = true;
                MoveSelectLeft.Enabled = true;
                MoveAllLeft.Enabled = true;
                MoveAllRight.Enabled = true;
                chooseDirectoryButton.Enabled = true;
                backupDirectoryTextBox.Enabled = true;
                filterTextBox.Enabled = true;
                backupProgressBar.Value = 100;
                progressBarLabel.Text = $"100% Complete";
                MessageBox.Show($"Backup Complete! You backed up {backupList.Items.Count} files!", "Backup Complete", MessageBoxButtons.OK);
                System.Diagnostics.Process.Start(backupDirectoryTextBox.Text);
            }
            else
                backgroundFinished = true;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i < backupList.Items.Count; i += 2)
            {
                if (connector2.Open())
                {
                    try
                    {
                        string sql = $"BACKUP DATABASE \"{backupList.Items[i]}\" TO DISK = \'{backupDirectoryTextBox.Text}\\{backupList.Items[i]}.BAK\' WITH INIT";
                        // string sql = $"BACKUP DATABASE \"{s}\" TO DISK = \'{backupDirectoryTextBox.Text}\\{s}.BAK\'";
                        var command = connector2.CreateCommand(sql);
                        command.CommandTimeout = 0;
                        var reader = connector2.ReadResults(command);
                        if (connector2.GetConnectionState() == ConnectionState.Open)
                        {
                            connector2.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        connector2.Close();
                        ef = new ErrorForm(ex);
                        ef.Show();
                        break;
                    }
                }
                int percentComplete = (int)(i / (float)(backupList.Items.Count) * 100);
                worker.ReportProgress(percentComplete);
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete3 = e.ProgressPercentage;
            int done = (percentComplete1 + percentComplete3) / 2;
            backupProgressBar.Value = done;
            Console.WriteLine($"{done}% FinishedBG3");
            progressBarLabel.Text = $"{done}% Complete";
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundFinished)
            {
                startBackUp.Enabled = true;
                RefreshDBs.Enabled = true;
                MoveSelectRight.Enabled = true;
                MoveSelectLeft.Enabled = true;
                MoveAllLeft.Enabled = true;
                MoveAllRight.Enabled = true;
                chooseDirectoryButton.Enabled = true;
                backupDirectoryTextBox.Enabled = true;
                filterTextBox.Enabled = true;
                backupProgressBar.Value = 100;
                progressBarLabel.Text = $"100% Complete";
                MessageBox.Show($"Backup Complete! You backed up {backupList.Items.Count} files!", "Backup Complete", MessageBoxButtons.OK);
                System.Diagnostics.Process.Start(backupDirectoryTextBox.Text);
            }
            else
                backgroundFinished = true;
        }
    }
}
