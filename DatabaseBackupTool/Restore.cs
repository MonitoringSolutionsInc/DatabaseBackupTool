using SqlConnector;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Restore : Form
    {
        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
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
                startRestore.Enabled = false;
                chooseDirectoryButton.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveBox.Checked);
            try
            {
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
                            ef.Show();
                        }
                    int percentComplete =
                                        (int)((float)i / (float)(filesToRestore.Length - 1) * 100); 
                    Console.WriteLine($"{percentComplete}% Finished");
                    worker.ReportProgress(percentComplete);
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine("Error caught");

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
            progressBar1.Value = 100;
            progressBarLabel.Text = $"100% Complete";
        }
    }
}
