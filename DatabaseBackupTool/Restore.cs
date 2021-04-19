using SqlConnector;
using System;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Restore : Form
    {
        private bool keepGoing = true;
        public bool validLocation;
        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            restoreDirectoryTextBox.Text = fbd.SelectedPath;
        }

        private void startRestore_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker1.IsBusy)
            {
                progressBar1.Value = 0;
                restoreDirectoryTextBox.Enabled = false;
                recursiveBox.Enabled = false;

                if (!Directory.Exists(restoreDirectoryTextBox.Text)) //if directory doesn't exist, set to some bogus place with no backup files and show error form
                {
                    restoreDirectoryTextBox.BackColor = Color.Red;
                    string myMessage = "The folowing directory could not be opened:\n" + restoreDirectoryTextBox.Text;
                    Exception myException = new Exception(myMessage);
                    ErrorForm ef = new ErrorForm(myException);
                    ef.FormClosed += new FormClosedEventHandler(errorBoxClosed);
                    ef.ShowDialog();
                }
                else
                {
                    startRestore.Enabled = false;
                    chooseDirectoryButton.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                }
            }

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveBox.Checked);
     
            string[] filesToRestore = Directory.GetFiles(restoreDirectoryTextBox.Text, "*.bak", recursive);

            for (int i = 0; i < filesToRestore.Length - 1; i++)
            {
                string databaseName = filesToRestore[i].Split('\\').Last().Split('.').First();
                string restoreSql = $@"RESTORE DATABASE [{databaseName}] FROM DISK='{filesToRestore[i]}' WITH REPLACE";
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
                    ErrorForm ef = new ErrorForm(ex);
                    //ef.Show();
                }
                int percentComplete = (int)((float)i / (float)(filesToRestore.Length - 1) * 100);
                Console.WriteLine($"{percentComplete}% Finished");
                worker.ReportProgress(percentComplete);
                }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            progressBarLabel.Text = $"{e.ProgressPercentage}% Complete";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startRestore.Enabled = true;
            chooseDirectoryButton.Enabled = true;
            recursiveBox.Enabled = true;
            restoreDirectoryTextBox.Enabled = true;
            progressBar1.Value = 100;
            progressBarLabel.Text = $"100% Complete";
        }


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (keepGoing)
            {
                if (Directory.Exists(restoreDirectoryTextBox.Text))
                    validLocation = true;
                else
                    validLocation = false;

                worker.ReportProgress(Convert.ToInt32(validLocation)); //raises the progressChanged Event which calls the function associated with that for this worker
            }
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (validLocation)
                restoreDirectoryTextBox.BackColor = Color.White;
            else
                restoreDirectoryTextBox.BackColor = Color.Red;
        }

        private void backgroundWorker2_killOnClose(FormClosedEventArgs e)
        {
            keepGoing = false;
            backgroundWorker2.CancelAsync();
        }


            private void errorBoxClosed(object sender, FormClosedEventArgs e)
        {
            restoreDirectoryTextBox.Enabled = true;
            recursiveBox.Enabled = true;
        }
    }
}
