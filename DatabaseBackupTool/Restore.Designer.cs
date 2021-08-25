
namespace DatabaseBackupTool
{
    partial class Restore
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
            this.chooseDirectoryButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.restoreDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.startRestore = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.recursiveBox = new System.Windows.Forms.CheckBox();
            this.backgroundWorkerRestore1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerPathCheck = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerRestore2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerRestore3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerRestore4 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBarLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chooseDirectoryButton
            // 
            this.chooseDirectoryButton.Location = new System.Drawing.Point(190, 72);
            this.chooseDirectoryButton.Name = "chooseDirectoryButton";
            this.chooseDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.chooseDirectoryButton.TabIndex = 27;
            this.chooseDirectoryButton.Text = "Path ...";
            this.chooseDirectoryButton.UseVisualStyleBackColor = true;
            this.chooseDirectoryButton.Click += new System.EventHandler(this.chooseDirectoryButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Restore Path";
            // 
            // restoreDirectoryTextBox
            // 
            this.restoreDirectoryTextBox.Location = new System.Drawing.Point(19, 46);
            this.restoreDirectoryTextBox.Name = "restoreDirectoryTextBox";
            this.restoreDirectoryTextBox.Size = new System.Drawing.Size(352, 20);
            this.restoreDirectoryTextBox.TabIndex = 25;
            this.restoreDirectoryTextBox.Text = "C:\\CEMDAS\\temp";
            // 
            // startRestore
            // 
            this.startRestore.Location = new System.Drawing.Point(271, 72);
            this.startRestore.Name = "startRestore";
            this.startRestore.Size = new System.Drawing.Size(104, 23);
            this.startRestore.TabIndex = 24;
            this.startRestore.Text = "Start Restore";
            this.startRestore.UseVisualStyleBackColor = true;
            this.startRestore.Click += new System.EventHandler(this.startRestore_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Select the path from which to restore";
            // 
            // recursiveBox
            // 
            this.recursiveBox.AutoSize = true;
            this.recursiveBox.Location = new System.Drawing.Point(104, 76);
            this.recursiveBox.Name = "recursiveBox";
            this.recursiveBox.Size = new System.Drawing.Size(74, 17);
            this.recursiveBox.TabIndex = 28;
            this.recursiveBox.Text = "Recursive";
            this.recursiveBox.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorkerRestore1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorkerRestore1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorkerRestore1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorkerPathCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorkerPathCheck.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorkerRestore2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorkerRestore2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorkerRestore2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // backgroundWorker4
            // 
            this.backgroundWorkerRestore3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.backgroundWorkerRestore3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker4_ProgressChanged);
            this.backgroundWorkerRestore3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker4_RunWorkerCompleted);
            // 
            // backgroundWorker5
            // 
            this.backgroundWorkerRestore4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker5_DoWork);
            this.backgroundWorkerRestore4.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker5_ProgressChanged);
            this.backgroundWorkerRestore4.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker5_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(19, 120);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(345, 23);
            this.progressBar1.TabIndex = 29;
            // 
            // progressBarLabel
            // 
            this.progressBarLabel.AutoSize = true;
            this.progressBarLabel.Location = new System.Drawing.Point(19, 101);
            this.progressBarLabel.Name = "progressBarLabel";
            this.progressBarLabel.Size = new System.Drawing.Size(48, 13);
            this.progressBarLabel.TabIndex = 30;
            this.progressBarLabel.Text = "Progress";
            // 
            // Restore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 155);
            this.Controls.Add(this.progressBarLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.recursiveBox);
            this.Controls.Add(this.chooseDirectoryButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.restoreDirectoryTextBox);
            this.Controls.Add(this.startRestore);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Restore";
            this.Text = "Database Restore Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Restore_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button chooseDirectoryButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox restoreDirectoryTextBox;
        private System.Windows.Forms.Button startRestore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox recursiveBox;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRestore1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerPathCheck;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRestore2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRestore3;
        private System.ComponentModel.BackgroundWorker backgroundWorkerRestore4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressBarLabel;
    }
}