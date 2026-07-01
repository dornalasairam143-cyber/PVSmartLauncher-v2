namespace PVSmartLauncher;

using System;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        // If PVSmartOpen.exe passed the file, or if dragged and dropped
        string targetFile = string.Empty;
        if (args.Length > 0)
        {
            targetFile = args[0];
        }

        Application.Run(new MainForm(targetFile));
    }
}
