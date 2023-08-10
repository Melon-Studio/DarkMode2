using Microsoft.Win32;
using MessageBox = DarkMode_2.Models.MessageBox;
using System;
using System.Diagnostics;
using System.Linq;
using log4net;
using DarkMode_2.Models;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetMore.xaml 的交互逻辑
/// </summary>
public partial class SetMore
{
    private static readonly ILog log = LogManager.GetLogger(typeof(SetMore));
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
            MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
            log.Error(ex.ToString());
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
        catch (Exception ex)
        {
            MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
            log.Error(ex);
        }
    }
}
