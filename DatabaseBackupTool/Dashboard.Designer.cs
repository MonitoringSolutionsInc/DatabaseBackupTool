
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
            this.SuspendLayout();
            // 
            // databaseBackupToolBtn
            // 
            this.databaseBackupToolBtn.Location = new System.Drawing.Point(13, 22);
            this.databaseBackupToolBtn.Name = "databaseBackupToolBtn";
            this.databaseBackupToolBtn.Size = new System.Drawing.Size(253, 23);
            this.databaseBackupToolBtn.TabIndex = 0;
            this.databaseBackupToolBtn.Text = "DatabaseBackupTool";
            this.databaseBackupToolBtn.UseVisualStyleBackColor = true;
            this.databaseBackupToolBtn.Click += new System.EventHandler(this.databaseBackupToolBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tools";
            // 
            // restoreBackupToolBtn
            // 
            this.restoreBackupToolBtn.Location = new System.Drawing.Point(12, 51);
            this.restoreBackupToolBtn.Name = "restoreBackupToolBtn";
            this.restoreBackupToolBtn.Size = new System.Drawing.Size(253, 23);
            this.restoreBackupToolBtn.TabIndex = 2;
            this.restoreBackupToolBtn.Text = "RestoreBackupTool";
            this.restoreBackupToolBtn.UseVisualStyleBackColor = true;
            this.restoreBackupToolBtn.Click += new System.EventHandler(this.restoreBackupToolBtn_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 99);
            this.Controls.Add(this.restoreBackupToolBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.databaseBackupToolBtn);
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Button databaseBackupToolBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button restoreBackupToolBtn;
    }
}