using Microsoft.Win32;
using MessageBox = DarkMode_2.Models.MessageBox;
using System;
using System.Diagnostics;
using System.Linq;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetMore.xaml 的交互逻辑
/// </summary>
public partial class SetMore
{
    public SetMore()
    {
        InitializeComponent();
    }

    public bool StartProcess(string filename, string args)
    {
        try
        {
            Process myprocess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(filename, args.Trim());
            myprocess.StartInfo = startInfo;
            myprocess.Start();
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.OpenMessageBox("错误发生", ex.ToString());
        }
        return false;
    }

    private void KillProcess(string name)
    {
        try
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process myProcess in myProcesses)
            {
                if (name == myProcess.ProcessName)
                    myProcess.Kill();
            }
        }
        catch (Exception e)
        { 
            Console.Error.WriteLine(e.ToString());
        }
    }
}
