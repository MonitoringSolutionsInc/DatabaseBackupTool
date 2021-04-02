using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void databaseBackupToolBtn_Click(object sender, EventArgs e)
        {
            Backup backup = new Backup();
            backup.ShowDialog();
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            Restore restore = new Restore();
            restore.ShowDialog();
        }
    }
}
