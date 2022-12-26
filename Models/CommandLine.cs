using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace DarkMode_2.Models;

public class CommandLine
{
    public static bool CallCommandLine(string hand, string command, string type)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if (getNowWallpaperPath(hand) == key.GetValue(type).ToString())
        {
            string zhilin = "\"" + hand + "\"" + " -control openWallpaper -file " + "\"" + command + "\"";
            Process process = new Process()
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.StandardInput.WriteLine(zhilin);
            string res = process.StandardOutput.ReadToEnd();
            process.StandardInput.WriteLine("exit");
            process.StandardInput.AutoFlush = true;
            process.WaitForExit();
            return true;
        }
        return false;
    }

    
    public static string getNowWallpaperPath(string hand)
    {
        //string zhilin = "\"" + hand + "\"" + " -control getWallpaper";
        ////执行cmd命令
        //Process CmdProcess = new Process();
        //CmdProcess.StartInfo.FileName = "cmd.exe";
        //CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
        //CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
        //CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
        //CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
        //CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
        //CmdProcess.StartInfo.Arguments = zhilin;//“/C”表示执行完命令后马上退出  
        //CmdProcess.StartInfo.RedirectStandardOutput = true;
        //CmdProcess.Start();//执行  

        //CmdProcess.StandardOutput.ReadToEnd();//获取返回值  
        //StreamReader sr = CmdProcess.StandardOutput;//获取返回值
        //CmdProcess.WaitForExit();//等待程序执行完退出进程  

        //CmdProcess.Close();//结束  

        //string line = sr.ReadLine();
        //return line;
        return "";
    }
}
