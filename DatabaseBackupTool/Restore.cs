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
        DateTime startTime;
        DateTime startTime1;
        DateTime startTime3;
        DateTime startTime4;
        DateTime startTime5;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Restore()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            backgroundWorkerRestore1.WorkerReportsProgress = true;
            backgroundWorkerPathCheck.WorkerReportsProgress = true;
            backgroundWorkerPathCheck.WorkerSupportsCancellation = true;
            backgroundWorkerRestore2.WorkerReportsProgress = true;
            backgroundWorkerRestore3.WorkerReportsProgress = true;
            backgroundWorkerRestore4.WorkerReportsProgress = true;
            backgroundWorkerPathCheck.RunWorkerAsync();
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            restoreDirectoryTextBox.Text = fbd.SelectedPath;
        }

        private void startRestore_Click(object sender, EventArgs e)
        {
            if(!backgroundWorkerRestore1.IsBusy)
            {
                progressBar1.Value = 0;
                restoreDirectoryTextBox.Enabled = false;
                recursiveBox.Enabled = false;

                if (!Directory.Exists(restoreDirectoryTextBox.Text)) //if directory doesn't exist, set to some bogus place with no backup files and show error form
                {
                    restoreDirectoryTextBox.BackColor = Color.Red;
                    string myMessage = "The following directory could not be opened:\n" + restoreDirectoryTextBox.Text;
                    Exception myException = new Exception(myMessage);
                    Logger.Error(myException);
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
                    progressBarLabel.Text = $"0% Complete";
                    progressBar1.Value = 0;
                    percentComplete1 = 0;
                    percentComplete3 = 0;
                    percentComplete4 = 0;
                    percentComplete5 = 0;
                    i = -1;
                    Logger.Info("Starting Background Workers ...");
                    backgroundWorkerRestore1.RunWorkerAsync();
                    backgroundWorkerRestore2.RunWorkerAsync();
                    backgroundWorkerRestore3.RunWorkerAsync();
                    backgroundWorkerRestore4.RunWorkerAsync();
                    backgroundWorkerPathCheck.CancelAsync();
                }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BG_DoWork(worker, "BG1");
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
            BG_DoWork(worker, "BG3");           
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
            BG_DoWork(worker, "BG4");
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
            BG_DoWork(worker, "BG5");
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
            while (!backgroundWorkerPathCheck.CancellationPending)
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

        private void errorBoxClosed(object sender, FormClosedEventArgs e)
        {
            restoreDirectoryTextBox.Enabled = true;
            recursiveBox.Enabled = true;
        }

        private void BG_DoWork(BackgroundWorker worker, String name)
        {
            SQLConnector conn = connectToSQL(); //create connection outside of loop for more reliable operation
            
            while (i < filesToRestore.Length)
            {
                lock (key)
                    i++;
                if (i < filesToRestore.Length) //perform this check here instead of inside lock to save on lock time
                {
                    restoreDatabase(i, filesToRestore, conn, name);
                    int percentComplete = (int)((float)i / (float)(filesToRestore.Length) * 100);
                    worker.ReportProgress(percentComplete);
                }
                else if (i == filesToRestore.Length) //when finished with restoring all files, check to make sure they succeeded
                {
                    stuckRestoringCheck(filesToRestore, conn);
                }
            }
            // Clean up SQLConnector.
            conn.Dispose();
            conn = null;
        }

        private void restoreDatabase(int i, string[] filesToRestore, SQLConnector conn, String workersName)
        {
            string databaseName = filesToRestore[i].Split('\\').Last().Split('.').First();
            string restoreSql = $@"RESTORE DATABASE [{databaseName}] FROM DISK='{filesToRestore[i]}' WITH REPLACE";
            Console.WriteLine($"{workersName} runs: Restoring {databaseName} using query: {restoreSql}");
            Logger.Info($"{workersName} runs: Restoring {databaseName} using query: {restoreSql}");
            try
            {
                conn.Open();
                var command = conn.CreateCommand(restoreSql);
                command.CommandTimeout = 0;
                conn.ReadResults(command);
                conn.Close();
            }
            catch (Exception ex)
            {
                if (conn != null && conn.GetConnectionState() == ConnectionState.Open)
                {
                    conn.Close();
                }
                Logger.Error(ex, $"An error occurred while attempting to restore {databaseName}");
            }
        }

        //checks over all databases that were restored and checks there state. If any are stuck in restoring state, then restore them again (single threaded at this point)
        private void stuckRestoringCheck(string[] filesToRestore, SQLConnector conn)
        {
            List<string> databases = new List<string>();
            List<string> fileList = new List<string>();
            
            foreach (var db in filesToRestore)
            {
                fileList.Add(db.Split('\\').Last().Split('.').First());
            }

            conn.Open();
            var dbResults = conn.ReadResults(conn.CreateCommand("SELECT NAME FROM SYS.DATABASES WHERE NAME NOT IN ('tempdb', 'master', 'model', 'msdb')"));
            while (dbResults.Read())
            {
                databases.Add(dbResults[0].ToString());
            }
            conn.Close();
            
            IEnumerable<string> dbInSqlAndInRestoreList = databases.Intersect<string>(fileList);
            
            while (WorkersAreFinishingUp());

            int i = 0;
            foreach (string dbName in dbInSqlAndInRestoreList)
            {
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
                    Logger.Info($"{dbName} currently in state: {result} - restoring... starting check over again");
                    restoreDatabase(i, filesToRestore, conn, "Final Check");
                    i = -1; //reset, start check over again to check this one again
                }
                i++;
            }
        }

        private bool WorkersAreFinishingUp()
        {
            int workersWorking = 0;
            if (backgroundWorkerRestore1.IsBusy)
                workersWorking++;
            if (backgroundWorkerRestore2.IsBusy)
                workersWorking++;
            if (backgroundWorkerRestore3.IsBusy)
                workersWorking++;
            if (backgroundWorkerRestore4.IsBusy)
                workersWorking++;

            if (workersWorking == 1)
                return false;
            return true;
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
            Logger.Info($"{work}% {who} restored a file in {time.Seconds}.{time.Milliseconds} seconds");
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
                Logger.Info($"Completed restore in {time.Minutes} minute(s) and {time.Seconds}.{time.Milliseconds} seconds");
                progressBar1.Value = 100;
                progressBarLabel.Text = $"100% Complete";
                backgroundWorkerPathCheck.RunWorkerAsync();
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
                Logger.Error(ex, $"An error occurred while attempting to estbalish SQL Connection. " +
                    $"Data Source: {Dashboard.SqlInfoData.Data_Source}, " +
                    $"Initial Catalog: {Dashboard.SqlInfoData.Initial_Catalog}, " +
                    $"User ID: {Dashboard.SqlInfoData.User_Id}, " +
                    $"Password: {Dashboard.SqlInfoData.Password}");
            }
            return conn;
        }

        private void Restore_FormClosing(object sender, FormClosingEventArgs e)
        {
            keepGoing = false;
            backgroundWorkerPathCheck.CancelAsync();
        }
    }
}
