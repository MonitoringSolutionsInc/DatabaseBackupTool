using SqlConnector;
using System;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DatabaseBackupTool
{
    public partial class Restore : Form
    {
        private bool keepGoing = true;
        string[] filesToRestore;
        public bool validLocation;
        private int percentComplete1 = 0;
        private int percentComplete3 = 0;
        private int percentComplete4 = 0;
        private int percentComplete5 = 0;
        private int backgroundFinished = 0;
        private readonly object key = new object();
        private int i = -1; //must start at -1 or program misses zero'th index of backups
        private Boolean checkDone = false;
        DateTime startTime;
        DateTime startTime1;
        DateTime startTime3;
        DateTime startTime4;
        DateTime startTime5;
        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker3.WorkerReportsProgress = true;
            backgroundWorker4.WorkerReportsProgress = true;
            backgroundWorker5.WorkerReportsProgress = true;
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
                    backgroundFinished = 0;
                    startTime  = DateTime.Now;
                    SearchOption recursive = (SearchOption)Convert.ToInt32(recursiveBox.Checked);
                    filesToRestore = Directory.GetFiles(restoreDirectoryTextBox.Text, "*.bak", recursive);
                    startTime1 = DateTime.Now;
                    startTime3 = DateTime.Now;
                    startTime4 = DateTime.Now;
                    startTime5 = DateTime.Now;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker3.RunWorkerAsync();
                    backgroundWorker4.RunWorkerAsync();
                    backgroundWorker5.RunWorkerAsync();
                }
            }

        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BG_DoWork(worker);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete1 = e.ProgressPercentage;
            ProgressChanged("BG1");
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            completed();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BG_DoWork(worker);           
        }
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete3 = e.ProgressPercentage;
            ProgressChanged("BG3");
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            completed();
        }


        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BG_DoWork(worker);
        }
        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete4 = e.ProgressPercentage;
            ProgressChanged("BG4");
        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            completed();
        }


        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BG_DoWork(worker);
        }
        
        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            percentComplete5 = e.ProgressPercentage;
            ProgressChanged("BG5");
        }
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            completed();
        }


        //Worker 2 is responsible for turning the path variable red if the path is incorrect in real time.
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

        private void BG_DoWork(BackgroundWorker worker)
        {
            SQLConnector conn = connectToSQL(); //create connection outside of loop for more reliable operation

            while (i < filesToRestore.Length)
            {
                lock (key)
                    i++;
                if (i < filesToRestore.Length) //perform this check here instead of inside lock to save on lock time
                {
                    restoreDatabase(i, filesToRestore, conn);
                    int percentComplete = (int)((float)i / (float)(filesToRestore.Length) * 100);
                    worker.ReportProgress(percentComplete);
                }
                else if (i == filesToRestore.Length) //when finished with restoring all files, check to make sure they succeeded
                {
                    stuckRestoringCheck(filesToRestore, conn);
                }
            }
        }


        private void restoreDatabase(int i, string[] filesToRestore, SQLConnector conn)
        {
            string databaseName = filesToRestore[i].Split('\\').Last().Split('.').First();
            string restoreSql = $@"RESTORE DATABASE [{databaseName}] FROM DISK='{filesToRestore[i]}' WITH REPLACE";
            Console.WriteLine($"Restoring {databaseName} using query: {restoreSql}");
            try
            {
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
                //ef.Show(); //Can't show error forms from the background workers, only from progress changed or complete
            }
        }


        //checks over all databases that were restored and checks there state. If any are stuck in restoring state, then restore them again (single threaded at this point)
        private void stuckRestoringCheck(string[] filesToRestore, SQLConnector conn)
        {
            conn.Open();

            var dbResults = conn.ReadResults(conn.CreateCommand("SELECT NAME FROM SYS.DATABASES WHERE NAME NOT IN ('tempdb', 'master', 'model', 'msdb')"));
            List<string> databases = new List<string>();
            while(dbResults.Read())
            {
                databases.Add(dbResults[0].ToString());
            }

            conn.Close();
            Boolean keepgoing = true;
            //make sure all other background workers are done with their restoring before proceeding
            while (keepgoing)
            {
                int workersWorking = 0;
                if (backgroundWorker1.IsBusy)
                    workersWorking++;                
                if (backgroundWorker3.IsBusy)
                    workersWorking++;
                if (backgroundWorker4.IsBusy)
                    workersWorking++;
                if (backgroundWorker5.IsBusy)
                    workersWorking++;

                if (workersWorking == 1)
                    keepgoing = false;
            }

            //Console.Clear(); //this line seems to cause problems for some reason. didn't investigate, just disabled since non-essential
            for (int i = 0; i < databases.Count; i++)
            {
                string dbName = databases[i];
                string sql_check_state = $"SELECT DATABASEPROPERTYEX('{dbName}', 'Status')";
                string result;

                if (conn.GetConnectionState() == ConnectionState.Open)
                {
                    conn.Close();
                }

                try
                {
                    conn.Open();
                    var reader = conn.ReadResults(conn.CreateCommand(sql_check_state));
                    reader.Read();
                    result = reader[0].ToString(); //read the result's first row grab its first column
                    Console.WriteLine(dbName + ": " + result);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    if (conn != null && conn.GetConnectionState() == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    result = "failed";
                }
                if (!result.Equals("ONLINE")) //if this database's state is not ONLINE then restore it again
                {
                    Console.WriteLine($"{dbName} currently in state: {result}");
                    Console.WriteLine("restoring... starting check over again");
                    restoreDatabase(i, filesToRestore, conn);
                    i = -1; //reset, start check over again to check this one again
                }
            }
        }

        private void ProgressChanged(string who)
        {
            TimeSpan time;
            int work = (percentComplete1 + percentComplete3 + percentComplete4 + percentComplete5) / 4;
            switch (who)
            {
                case "BG1":
                    time = DateTime.Now - startTime1;
                    startTime1 = DateTime.Now;
                    break;
                case "BG3":
                    time = DateTime.Now - startTime3;
                    startTime3 = DateTime.Now;
                    break;
                case "BG4":
                    time = DateTime.Now - startTime4;
                    startTime4 = DateTime.Now;
                    break;
                case "BG5":
                    time = DateTime.Now - startTime5;
                    startTime5 = DateTime.Now;
                    break;
                default:
                    Console.WriteLine("Idk what happened");
                    time = DateTime.Now - startTime;
                    break;
            }                


            Console.WriteLine($"{work}% {who} restored a file in {time.Seconds}.{time.Milliseconds} seconds");
            progressBar1.Value = work;
            progressBarLabel.Text = $"{work}% Complete";
        }
        private void completed()
        {
            if (backgroundFinished >= 3) //run only when all threads are done
            {
                TimeSpan time = DateTime.Now - startTime;
                startRestore.Enabled = true;
                chooseDirectoryButton.Enabled = true;
                recursiveBox.Enabled = true;
                restoreDirectoryTextBox.Enabled = true;
                Console.WriteLine($"completed restore in {time.Minutes} minute(s) and {time.Seconds}.{time.Milliseconds} seconds");
                progressBar1.Value = 100;
                progressBarLabel.Text = $"100% Complete";
            }
            else
                backgroundFinished++;
        }
        private SQLConnector connectToSQL()
        {             
            SQLConnector conn = null;
            try
            {
                conn = new SQLConnector(Dashboard.SqlInfoData.Data_Source, Dashboard.SqlInfoData.Initial_Catalog, Dashboard.SqlInfoData.User_Id, Dashboard.SqlInfoData.Password);
                conn.InitializeConnection();
            }
            catch (Exception ex)
            {
                if (conn != null && conn.GetConnectionState() == ConnectionState.Open)
                {
                    conn.Close();
                }
                ErrorForm ef = new ErrorForm(ex);
                //ef.Show(); //Can't show error forms from the background workers, only from progress changed or complete
            }
            return conn;
        }
    }
}
