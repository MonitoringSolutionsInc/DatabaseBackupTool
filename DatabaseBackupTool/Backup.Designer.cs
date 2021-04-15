namespace DatabaseBackupTool
{
    partial class Backup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RefreshDBs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.databaseList = new System.Windows.Forms.ListBox();
            this.backupList = new System.Windows.Forms.ListBox();
            this.MoveSelectRight = new System.Windows.Forms.Button();
            this.MoveSelectLeft = new System.Windows.Forms.Button();
            this.MoveAllRight = new System.Windows.Forms.Button();
            this.MoveAllLeft = new System.Windows.Forms.Button();
            this.startBackUp = new System.Windows.Forms.Button();
            this.backupDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chooseDirectoryButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.backupProgressBar = new System.Windows.Forms.ProgressBar();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RefreshDBs
            // 
            this.RefreshDBs.Location = new System.Drawing.Point(12, 35);
            this.RefreshDBs.Name = "RefreshDBs";
            this.RefreshDBs.Size = new System.Drawing.Size(95, 23);
            this.RefreshDBs.TabIndex = 0;
            this.RefreshDBs.Text = "Refresh List";
            this.RefreshDBs.UseVisualStyleBackColor = true;
            this.RefreshDBs.Click += new System.EventHandler(this.RefreshDBs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select the databases to be backed up.";
            // 
            // databaseList
            // 
            this.databaseList.FormattingEnabled = true;
            this.databaseList.Location = new System.Drawing.Point(12, 82);
            this.databaseList.Name = "databaseList";
            this.databaseList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.databaseList.Size = new System.Drawing.Size(120, 225);
            this.databaseList.Sorted = true;
            this.databaseList.TabIndex = 2;
            this.databaseList.SelectedIndexChanged += new System.EventHandler(this.databaseList_SelectedIndexChanged);
            // 
            // backupList
            // 
            this.backupList.FormattingEnabled = true;
            this.backupList.Location = new System.Drawing.Point(248, 82);
            this.backupList.Name = "backupList";
            this.backupList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.backupList.Size = new System.Drawing.Size(120, 225);
            this.backupList.Sorted = true;
            this.backupList.TabIndex = 3;
            // 
            // MoveSelectRight
            // 
            this.MoveSelectRight.Location = new System.Drawing.Point(138, 82);
            this.MoveSelectRight.Name = "MoveSelectRight";
            this.MoveSelectRight.Size = new System.Drawing.Size(104, 23);
            this.MoveSelectRight.TabIndex = 4;
            this.MoveSelectRight.Text = ">";
            this.MoveSelectRight.UseVisualStyleBackColor = true;
            this.MoveSelectRight.Click += new System.EventHandler(this.MoveSelectRight_Click);
            // 
            // MoveSelectLeft
            // 
            this.MoveSelectLeft.Location = new System.Drawing.Point(138, 111);
            this.MoveSelectLeft.Name = "MoveSelectLeft";
            this.MoveSelectLeft.Size = new System.Drawing.Size(104, 23);
            this.MoveSelectLeft.TabIndex = 5;
            this.MoveSelectLeft.Text = "<";
            this.MoveSelectLeft.UseVisualStyleBackColor = true;
            this.MoveSelectLeft.Click += new System.EventHandler(this.MoveSelectLeft_Click);
            // 
            // MoveAllRight
            // 
            this.MoveAllRight.Location = new System.Drawing.Point(138, 150);
            this.MoveAllRight.Name = "MoveAllRight";
            this.MoveAllRight.Size = new System.Drawing.Size(104, 23);
            this.MoveAllRight.TabIndex = 6;
            this.MoveAllRight.Text = ">>";
            this.MoveAllRight.UseVisualStyleBackColor = true;
            this.MoveAllRight.Click += new System.EventHandler(this.MoveAllRight_Click);
            // 
            // MoveAllLeft
            // 
            this.MoveAllLeft.Location = new System.Drawing.Point(138, 179);
            this.MoveAllLeft.Name = "MoveAllLeft";
            this.MoveAllLeft.Size = new System.Drawing.Size(104, 23);
            this.MoveAllLeft.TabIndex = 7;
            this.MoveAllLeft.Text = "<<";
            this.MoveAllLeft.UseVisualStyleBackColor = true;
            this.MoveAllLeft.Click += new System.EventHandler(this.MoveAllLeft_Click);
            // 
            // startBackUp
            // 
            this.startBackUp.Location = new System.Drawing.Point(264, 353);
            this.startBackUp.Name = "startBackUp";
            this.startBackUp.Size = new System.Drawing.Size(104, 23);
            this.startBackUp.TabIndex = 8;
            this.startBackUp.Text = "Start Backup";
            this.startBackUp.UseVisualStyleBackColor = true;
            this.startBackUp.Click += new System.EventHandler(this.startBackUp_Click);
            // 
            // backupDirectoryTextBox
            // 
            this.backupDirectoryTextBox.Location = new System.Drawing.Point(16, 330);
            this.backupDirectoryTextBox.Name = "backupDirectoryTextBox";
            this.backupDirectoryTextBox.Size = new System.Drawing.Size(352, 20);
            this.backupDirectoryTextBox.TabIndex = 9;
            this.backupDirectoryTextBox.Text = "C:\\CEMDAS\\temp";
            this.backupDirectoryTextBox.TextChanged += new System.EventHandler(this.backupDirectoryTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Backup Path";
            // 
            // chooseDirectoryButton
            // 
            this.chooseDirectoryButton.Location = new System.Drawing.Point(183, 353);
            this.chooseDirectoryButton.Name = "chooseDirectoryButton";
            this.chooseDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.chooseDirectoryButton.TabIndex = 11;
            this.chooseDirectoryButton.Text = "Path ...";
            this.chooseDirectoryButton.UseVisualStyleBackColor = true;
            this.chooseDirectoryButton.Click += new System.EventHandler(this.chooseDirectoryButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Databases On Disk";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "To Backup";
            // 
            // backupProgressBar
            // 
            this.backupProgressBar.Location = new System.Drawing.Point(19, 396);
            this.backupProgressBar.Name = "backupProgressBar";
            this.backupProgressBar.Size = new System.Drawing.Size(349, 23);
            this.backupProgressBar.TabIndex = 14;
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.AutoSize = true;
            this.progressBarLabel.Location = new System.Drawing.Point(19, 377);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(48, 13);
            this.progressBarLabel.TabIndex = 15;
            this.progressBarLabel.Text = "Progress";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(251, 35);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(100, 20);
            this.filterTextBox.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Filter Selection:";
            // 
            // Backup
            // 
            this.AcceptButton = this.RefreshDBs;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 431);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.backupProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chooseDirectoryButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.backupDirectoryTextBox);
            this.Controls.Add(this.startBackUp);
            this.Controls.Add(this.MoveAllLeft);
            this.Controls.Add(this.MoveAllRight);
            this.Controls.Add(this.MoveSelectLeft);
            this.Controls.Add(this.MoveSelectRight);
            this.Controls.Add(this.backupList);
            this.Controls.Add(this.databaseList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RefreshDBs);
            this.MaximizeBox = false;
            this.Name = "Backup";
            this.Text = "Database Backup Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RefreshDBs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox databaseList;
        private System.Windows.Forms.ListBox backupList;
        private System.Windows.Forms.Button MoveSelectRight;
        private System.Windows.Forms.Button MoveSelectLeft;
        private System.Windows.Forms.Button MoveAllRight;
        private System.Windows.Forms.Button MoveAllLeft;
        private System.Windows.Forms.Button startBackUp;
        private System.Windows.Forms.TextBox backupDirectoryTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button chooseDirectoryButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar backupProgressBar;
        private System.Windows.Forms.Label progressBarLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label label5;
    }
}

