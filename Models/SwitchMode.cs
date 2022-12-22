using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DarkMode_2.Models;

public class SwitchMode
{
    public static void switchMode(string mode)
    {
        //注册表变量初始化
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        RegistryKey sysKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);
        //切换主题色
        if(mode == "light" && DetermineSystemColorMode.GetSysState() == "dark")
        {
            sysKey.SetValue("SystemUsesLightTheme", Convert.ToInt32("1", 16));
            sysKey.SetValue("AppsUseLightTheme", Convert.ToInt32("1", 16));
            Console.WriteLine(RedRawWindow.ChangeColorMode());
            if (key.GetValue("NativeDark").ToString() != "")
            {
                string path = key.GetValue("NativeDark").ToString();
                ChangeNativeWallpaper(path);
            }
        }else if (mode == "dark" && DetermineSystemColorMode.GetSysState() == "light")
        {
            sysKey.SetValue("SystemUsesLightTheme", Convert.ToInt32("0", 16));
            sysKey.SetValue("AppsUseLightTheme", Convert.ToInt32("0", 16));
            Console.WriteLine(RedRawWindow.ChangeColorMode().ToString());
            if (key.GetValue("NativeLight").ToString() != "")
            {
                string path = (string)key.GetValue("NativeLight");
                ChangeNativeWallpaper(path);
            }
        }
        key.Close();
        sysKey.Close();
    }

    //切换原生壁纸
    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    public static void ChangeNativeWallpaper(string FilePath)
    {
        string filePath = Path.GetFullPath(FilePath);
        SystemParametersInfo(20, 1, FilePath, 0x1 | 0x2);
    }
    //切换WE壁纸


    
}
