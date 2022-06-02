using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
internal static class FolderBrowserDialogExtension
{
    public static DialogResult ShowDialog(this FolderBrowserDialog dialog, bool scrollIntoView)
    {
        return ShowDialog(dialog, null, scrollIntoView);
    }

    public static DialogResult ShowDialog(this FolderBrowserDialog dialog, IWin32Window owner, bool scrollIntoView)
    {
        if (scrollIntoView)
        {
            SendKeys.Send("{TAB}{TAB}{RIGHT}");
        }

        return dialog.ShowDialog(owner);
    }
}
}
