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
        private int percentComplete1 = 0;
        private int percentComplete3 = 0;
        private bool backgroundFinished = false;
        DateTime startTime;
        DateTime startTime1;
        DateTime startTime3;
        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker3.WorkerReportsProgress = true;
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
                    string myMessage = "The following directory could not be opened:\n" + restoreDirectoryTextBox.Text;
                    Exception myException = new Exception(myMessage);
                    ErrorForm ef = new ErrorForm(myException);
                    ef.FormClosed += new FormClosedEventHandler(errorBoxClosed);
                    ef.ShowDialog();
                }
                else
                {
                    startRestore.Enabled = false;
                    chooseDirectoryButton.Enabled = false;
                    backgroundFinished = false;
                    startTime1 = DateTime.Now;
                    startTime3 = DateTime.Now;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker3.RunWorkerAsync();
                }
            }

        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveBox.Checked);
     
            string[] filesToRestore = Directory.GetFiles(restoreDirectoryTextBox.Text, "*.bak", recursive);

            for (int i = 0; i < filesToRestore.Length; i += 2)//for (int i = 0; i < filesToRestore.Length - 1; i += 2)
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
                int percentComplete = (int)((float)i / (float)(filesToRestore.Length) * 100);
                worker.ReportProgress(percentComplete);
                }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete1 = e.ProgressPercentage; 
            int work = (percentComplete1 + percentComplete3) / 2;
            TimeSpan time = DateTime.Now - startTime3;
            startTime1 = DateTime.Now;

            Console.WriteLine($"{work}% BG1 restored a file in {time.Milliseconds}ms");
            progressBar1.Value = work;
            progressBarLabel.Text = $"{work}% Complete";
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundFinished)
            {
                TimeSpan time = DateTime.Now - startTime;
                startRestore.Enabled = true;
                chooseDirectoryButton.Enabled = true;
                recursiveBox.Enabled = true;
                restoreDirectoryTextBox.Enabled = true;
                Console.WriteLine($"completed restore in {time.Seconds}seconds");
                progressBar1.Value = 100;
                progressBarLabel.Text = $"100% Complete";
            }
            else
                backgroundFinished = true;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveBox.Checked);

            string[] filesToRestore = Directory.GetFiles(restoreDirectoryTextBox.Text, "*.bak", recursive);

            for (int i = 1; i < filesToRestore.Length; i += 2) //for (int i = 1; i < filesToRestore.Length - 1; i += 2)
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
                int percentComplete = (int)((float)i / (float)(filesToRestore.Length) * 100);
                worker.ReportProgress(percentComplete);
            }
        }
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete3 = e.ProgressPercentage;
            int work = (percentComplete1 + percentComplete3) / 2;
            TimeSpan time = DateTime.Now - startTime3;
            startTime3 = DateTime.Now;

            Console.WriteLine($"{work}% BG3 restored a file in {time.Milliseconds}ms");
            progressBar1.Value = work;            
            progressBarLabel.Text = $"{work}% Complete";
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundFinished)
            {
                TimeSpan time = DateTime.Now - startTime;
                startRestore.Enabled = true;
                chooseDirectoryButton.Enabled = true;
                recursiveBox.Enabled = true;
                restoreDirectoryTextBox.Enabled = true;
                Console.WriteLine($"completed restore in {time.Seconds}seconds");
                progressBar1.Value = 100;
                progressBarLabel.Text = $"100% Complete";
            }
            else
                backgroundFinished = true;
        }


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (keepGoing)
            {
                System.Threading.Thread.Sleep(100); //change update rate of text box in milliseconds (cannot be zero)
                worker.ReportProgress(85); //raises the progressChanged Event which calls the function associated with that for this worker
            }
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Directory.Exists(restoreDirectoryTextBox.Text))
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
