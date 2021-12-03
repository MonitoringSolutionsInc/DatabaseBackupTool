
using System;

namespace DatabaseBackupTool
{
    partial class Dashboard
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
            this.databaseBackupToolBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.restoreBackupToolBtn = new System.Windows.Forms.Button();
            this.ServerInstanceComboBox = new System.Windows.Forms.ComboBox();
            this.LoadSqlServerInstancesAsync = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // databaseBackupToolBtn
            // 
            this.databaseBackupToolBtn.Location = new System.Drawing.Point(13, 16);
            this.databaseBackupToolBtn.Name = "databaseBackupToolBtn";
            this.databaseBackupToolBtn.Size = new System.Drawing.Size(253, 23);
            this.databaseBackupToolBtn.TabIndex = 1;
            this.databaseBackupToolBtn.Text = "DatabaseBackupTool";
            this.databaseBackupToolBtn.UseVisualStyleBackColor = true;
            this.databaseBackupToolBtn.Click += new System.EventHandler(this.databaseBackupToolBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tools";
            // 
            // restoreBackupToolBtn
            // 
            this.restoreBackupToolBtn.Location = new System.Drawing.Point(13, 44);
            this.restoreBackupToolBtn.Name = "restoreBackupToolBtn";
            this.restoreBackupToolBtn.Size = new System.Drawing.Size(253, 23);
            this.restoreBackupToolBtn.TabIndex = 2;
            this.restoreBackupToolBtn.Text = "RestoreBackupTool";
            this.restoreBackupToolBtn.UseVisualStyleBackColor = true;
            this.restoreBackupToolBtn.Click += new System.EventHandler(this.restoreBackupToolBtn_Click);
            // 
            // ServerInstanceComboBox
            // 
            this.ServerInstanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerInstanceComboBox.FormattingEnabled = true;
            this.ServerInstanceComboBox.Location = new System.Drawing.Point(14, 73);
            this.ServerInstanceComboBox.Name = "ServerInstanceComboBox";
            this.ServerInstanceComboBox.Size = new System.Drawing.Size(251, 21);
            this.ServerInstanceComboBox.TabIndex = 3;
            this.ServerInstanceComboBox.SelectionChangeCommitted += new System.EventHandler(this.ServerInstanceComboBox_SelectionChangeCommitted);
            // 
            // LoadSqlServerInstancesAsync
            // 
            this.LoadSqlServerInstancesAsync.WorkerReportsProgress = true;
            this.LoadSqlServerInstancesAsync.WorkerSupportsCancellation = true;
            this.LoadSqlServerInstancesAsync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadSqlServerInstancesAsync_DoWork);
            this.LoadSqlServerInstancesAsync.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadSqlServerInstancesAsync_ProgressChanged);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 99);
            this.Controls.Add(this.ServerInstanceComboBox);
            this.Controls.Add(this.restoreBackupToolBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.databaseBackupToolBtn);
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Button databaseBackupToolBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button restoreBackupToolBtn;
        private System.Windows.Forms.ComboBox ServerInstanceComboBox;
        private System.ComponentModel.BackgroundWorker LoadSqlServerInstancesAsync;
    }
}