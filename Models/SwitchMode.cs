using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;

namespace DarkMode_2.Models;

public class SwitchMode
{
    public async static void switchMode(string mode)
    {
        //注册表变量初始化
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        RegistryKey sysKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);
        RegistryKey cursorKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);
        RegistryKey keyboard1 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\TabletTip\1.7\SelectedThemeDark", true);
        RegistryKey keyboard2 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\TabletTip\1.7\SelectedThemeLight", true);


        string WeInstallPath = key.GetValue("WeInstallPath").ToString();
        //切换主题色
        if (mode == "light" && DetermineSystemColorMode.GetSysState() == "dark") //浅色
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
            if (key.GetValue("WeLight").ToString() != "")
            {
                if (key.GetValue("WeInstallPath").ToString() != "")
                {
                    Console.WriteLine("Wallpaper TEST: " + WallpaperChanger.GetWallpaperPath(WeInstallPath));
                    WallpaperChanger.SetWallpaper(WeInstallPath, key.GetValue("WeLight").ToString());
                }
            }
            //Mouse Cursor
            if(key.GetValue("SwitchMouse").ToString() == "true")
            {
                if (key.GetValue("LightMouse").ToString() == "Light")
                {
                    cursorKey.SetValue("(Default)", @"Windows Aero");
                    cursorKey.SetValue("Arrow", @"%SystemRoot%\cursors\aero_arrow.cur");
                    cursorKey.SetValue("Help", @"%SystemRoot%\cursors\aero_helpsel.cur");
                    cursorKey.SetValue("AppStarting", @"%SystemRoot%\cursors\aero_working.ani");
                    cursorKey.SetValue("Wait", @"%SystemRoot%\cursors\aero_busy.ani");
                    cursorKey.SetValue("Crosshair", @"");
                    cursorKey.SetValue("IBeam", @"");
                    cursorKey.SetValue("NWPen", @"%SystemRoot%\cursors\aero_pen.cur");
                    cursorKey.SetValue("No", @"%SystemRoot%\cursors\aero_unavail.cur");
                    cursorKey.SetValue("SizeNS", @"%SystemRoot%\cursors\aero_ns.cur");
                    cursorKey.SetValue("SizeWE", @"%SystemRoot%\cursors\aero_ew.cur");
                    cursorKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\aero_nwse.cur");
                    cursorKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\aero_nesw.cur");
                    cursorKey.SetValue("SizeAll", @"%SystemRoot%\cursors\aero_move.cur");
                    cursorKey.SetValue("UpArrow", @"%SystemRoot%\cursors\aero_up.cur");
                    cursorKey.SetValue("Hand", @"%SystemRoot%\cursors\aero_link.cur");
                    cursorKey.SetValue("Scheme Source", Convert.ToInt32("1", 2));
                    RedRawWindow.RefreshSystemScheme();
                }
                else
                {
                    cursorKey.SetValue("(Default)", @"Windows Black");
                    cursorKey.SetValue("Arrow", @"%SystemRoot%\cursors\arrow_r.cur");
                    cursorKey.SetValue("Help", @"%SystemRoot%\cursors\help_r.cur");
                    cursorKey.SetValue("AppStarting", @"%SystemRoot%\cursors\wait_r.cur");
                    cursorKey.SetValue("Wait", @"%SystemRoot%\cursors\busy_r.cur");
                    cursorKey.SetValue("Crosshair", @"%SystemRoot%\cursors\cross_r.cur");
                    cursorKey.SetValue("IBeam", @"%SystemRoot%\cursors\beam_r.cur");
                    cursorKey.SetValue("NWPen", @"%SystemRoot%\cursors\pen_r.cur");
                    cursorKey.SetValue("No", @"%SystemRoot%\cursors\no_r.cur");
                    cursorKey.SetValue("SizeNS", @"%SystemRoot%\cursors\size4_r.cur");
                    cursorKey.SetValue("SizeWE", @"%SystemRoot%\cursors\size3_r.cur");
                    cursorKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\size2_r.cur");
                    cursorKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\size1_r.cur");
                    cursorKey.SetValue("SizeAll", @"%SystemRoot%\cursors\move_r.cur");
                    cursorKey.SetValue("UpArrow", @"%SystemRoot%\cursors\up_r.cur");
                    cursorKey.SetValue("Hand", @"");
                    cursorKey.SetValue("Scheme Source", Convert.ToInt32("1", 2));
                    RedRawWindow.RefreshSystemScheme();
                }
            }
            
            //触摸键盘
            if(key.GetValue("KeyboardMode").ToString() == "true")
            {
                keyboard1.SetValue("ThemeOverride", "Light");
                keyboard2.SetValue("ThemeOverride", "Light");
                keyboard1.SetValue("KeyboardBackgroundSolidColor", "243,243,243");
                keyboard2.SetValue("KeyboardBackgroundSolidColor", "243,243,243");
                keyboard1.SetValue("KeyLabelColor", "0,0,0");
                keyboard2.SetValue("KeyLabelColor", "0,0,0");
                keyboard1.SetValue("SuggestionTextColor", "0,0,0");
                keyboard2.SetValue("SuggestionTextColor", "0,0,0");
                keyboard1.SetValue("KeyTransparency", Convert.ToInt32("1", 0));
                keyboard2.SetValue("KeyTransparency", Convert.ToInt32("1", 0));
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
            if (key.GetValue("WeDark").ToString() != "")
            {
                if (key.GetValue("WeInstallPath").ToString() != "")
                {

                    Console.WriteLine("Wallpaper TEST: "+WallpaperChanger.GetWallpaperPath(WeInstallPath));
                    WallpaperChanger.SetWallpaper(WeInstallPath, key.GetValue("WeDark").ToString());
                }
            }
            //Mouse Cursor
            if (key.GetValue("SwitchMouse").ToString() == "true")
            {
                if (key.GetValue("DarkMouse").ToString() == "Light")
                {
                    cursorKey.SetValue("(Default)", @"Windows Aero");
                    cursorKey.SetValue("Arrow", @"%SystemRoot%\cursors\aero_arrow.cur");
                    cursorKey.SetValue("Help", @"%SystemRoot%\cursors\aero_helpsel.cur");
                    cursorKey.SetValue("AppStarting", @"%SystemRoot%\cursors\aero_working.ani");
                    cursorKey.SetValue("Wait", @"%SystemRoot%\cursors\aero_busy.ani");
                    cursorKey.SetValue("Crosshair", @"");
                    cursorKey.SetValue("IBeam", @"");
                    cursorKey.SetValue("NWPen", @"%SystemRoot%\cursors\aero_pen.cur");
                    cursorKey.SetValue("No", @"%SystemRoot%\cursors\aero_unavail.cur");
                    cursorKey.SetValue("SizeNS", @"%SystemRoot%\cursors\aero_ns.cur");
                    cursorKey.SetValue("SizeWE", @"%SystemRoot%\cursors\aero_ew.cur");
                    cursorKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\aero_nwse.cur");
                    cursorKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\aero_nesw.cur");
                    cursorKey.SetValue("SizeAll", @"%SystemRoot%\cursors\aero_move.cur");
                    cursorKey.SetValue("UpArrow", @"%SystemRoot%\cursors\aero_up.cur");
                    cursorKey.SetValue("Hand", @"%SystemRoot%\cursors\aero_link.cur");
                    cursorKey.SetValue("Scheme Source", Convert.ToInt32("1", 2));
                    RedRawWindow.RefreshSystemScheme();
                }
                else
                {
                    cursorKey.SetValue("(Default)", @"Windows Black");
                    cursorKey.SetValue("Arrow", @"%SystemRoot%\cursors\arrow_r.cur");
                    cursorKey.SetValue("Help", @"%SystemRoot%\cursors\help_r.cur");
                    cursorKey.SetValue("AppStarting", @"%SystemRoot%\cursors\wait_r.cur");
                    cursorKey.SetValue("Wait", @"%SystemRoot%\cursors\busy_r.cur");
                    cursorKey.SetValue("Crosshair", @"%SystemRoot%\cursors\cross_r.cur");
                    cursorKey.SetValue("IBeam", @"%SystemRoot%\cursors\beam_r.cur");
                    cursorKey.SetValue("NWPen", @"%SystemRoot%\cursors\pen_r.cur");
                    cursorKey.SetValue("No", @"%SystemRoot%\cursors\no_r.cur");
                    cursorKey.SetValue("SizeNS", @"%SystemRoot%\cursors\size4_r.cur");
                    cursorKey.SetValue("SizeWE", @"%SystemRoot%\cursors\size3_r.cur");
                    cursorKey.SetValue("SizeNWSE", @"%SystemRoot%\cursors\size2_r.cur");
                    cursorKey.SetValue("SizeNESW", @"%SystemRoot%\cursors\size1_r.cur");
                    cursorKey.SetValue("SizeAll", @"%SystemRoot%\cursors\move_r.cur");
                    cursorKey.SetValue("UpArrow", @"%SystemRoot%\cursors\up_r.cur");
                    cursorKey.SetValue("Hand", @"");
                    cursorKey.SetValue("Scheme Source", Convert.ToInt32("1", 2));
                    RedRawWindow.RefreshSystemScheme();
                }
            }
            
            //触摸键盘
            if (key.GetValue("KeyboardMode").ToString() == "true")
            {
                keyboard1.SetValue("ThemeOverride", "Dark");
                keyboard2.SetValue("ThemeOverride", "Dark");
                keyboard1.SetValue("KeyboardBackgroundSolidColor", "28,28,28");
                keyboard2.SetValue("KeyboardBackgroundSolidColor", "28,28,28");
                keyboard1.SetValue("KeyLabelColor", "255,255,255");
                keyboard2.SetValue("KeyLabelColor", "255,255,255");
                keyboard1.SetValue("SuggestionTextColor", "255,255,255");
                keyboard2.SetValue("SuggestionTextColor", "255,255,255");
                keyboard1.SetValue("KeyTransparency", Convert.ToInt32("1", 57));
                keyboard2.SetValue("KeyTransparency", Convert.ToInt32("1", 57));
            }
        }

        //自动更新
        //Update update = new Update();
        //string res = await update.CheckUpdate();
        //Match match = Regex.Match(res, @"\d+\.\d+\.\d+\.\d+-\w+");
        //if (match.Success)
        //{
        //    DownloadManager download = new DownloadManager();
        //    download.DownloadCompleted += DownloadManager_DownloadCompleted;
        //    string url = new JsonSerialization().FileDownloadUrl(res);
        //    await download.DownloadFileAsync(url);
        //}

        key.Close();
        sysKey.Close();
        cursorKey.Close();
    }

    private static void DownloadManager_DownloadCompleted(string obj)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("InstallUpdate", "true");
        key.Close();
    }
}
