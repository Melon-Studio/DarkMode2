using Microsoft.Win32;

namespace DarkMode_2.Models;

public class DetermineSystemColorMode
{
    //获取系统用户APP主题色
    public static string GetState()
    {
        string state = null;
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false);
        if (key.GetValue("AppsUseLightTheme").ToString() == "1")
        {
            state = "light";
        }
        else
        {
            state = "dark";
        }
        key.Close();
        return state;
    }

    //获取系统用户主题色
    public static string GetSysState()
    {
        string state = null;
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false);
        if (key.GetValue("SystemUsesLightTheme").ToString() == "1")
        {
            state = "light";
        }
        else
        {
            state = "dark";
        }
        key.Close();
        return state;
    }
}
