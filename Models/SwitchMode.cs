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
        if(mode == "light" && DetermineSystemColorMode.GetSysState() == "dark") //浅色
        {
            //Colors mode
            sysKey.SetValue("SystemUsesLightTheme", Convert.ToInt32("1", 16));
            sysKey.SetValue("AppsUseLightTheme", Convert.ToInt32("1", 16));
            RedRawWindow.ChangeColorMode();
            //wallpaper
            if (key.GetValue("NativeLight").ToString() != "")
            {
                string path = key.GetValue("NativeLight").ToString();
                ReplaceWallpaper.ChangeNativeWallpaper(path);
            }
            //wallpaper engine
            if (key.GetValue("WeDark").ToString() != "")
            {
                if (key.GetValue("WeInstallPath").ToString() != "")
                {
                    WallpaperChanger wallpaperChanger = new(key.GetValue("WeInstallPath").ToString(), key.GetValue("WeDark").ToString());
                    wallpaperChanger.openWallpaper();

                }
            }
        }
        else if (mode == "dark" && DetermineSystemColorMode.GetSysState() == "light") //深色
        {
            //color mode
            sysKey.SetValue("SystemUsesLightTheme", Convert.ToInt32("0", 16));
            sysKey.SetValue("AppsUseLightTheme", Convert.ToInt32("0", 16));
            RedRawWindow.ChangeColorMode().ToString();
            //wallpaper
            if (key.GetValue("NativeDark").ToString() != "")
            {
                string path = (string)key.GetValue("NativeDark");
                ReplaceWallpaper.ChangeNativeWallpaper(path);
            }
            //wallpaper engine
            if (key.GetValue("WeLight").ToString() != "")
            {
                if (key.GetValue("WeInstallPath").ToString() != "")
                {
                    WallpaperChanger wallpaperChanger = new(key.GetValue("WeInstallPath").ToString(), key.GetValue("WeLight").ToString());
                    wallpaperChanger.openWallpaper();
                }
            }
        }
        key.Close();
        sysKey.Close();
    }

}
