using System;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(Exception e)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Text = $"{e.GetType()} Error";
            richTextBox1.Text = e.Message;
        }

        private void ErrorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
