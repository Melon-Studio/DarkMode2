using DarkMode_2.Models;
using Microsoft.Win32;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetDIY.xaml 的交互逻辑
/// </summary>
public partial class SetDIY
{
    public SetDIY()
    {
        InitializeComponent();
        //设置初始化
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if (key.GetValue("LightMouse").ToString() == "Light")
        {
            LightMouse_white.IsChecked = true;
            LightMouse_black.IsChecked = false;
        }
        else if(key.GetValue("LightMouse").ToString() == "Dark")
        {
            LightMouse_black.IsChecked = true;
            LightMouse_white.IsChecked = false;
        }
        if(key.GetValue("DarkMouse").ToString() == "Light")
        {
            DarkMouse_white.IsChecked = true;
            DarkMouse_black.IsChecked = false;
        }
        else if(key.GetValue("DarkMouse").ToString() == "Dark")
        {
            DarkMouse_black.IsChecked = true;
            DarkMouse_white.IsChecked = false;
        }
        if(key.GetValue("SwitchMouse").ToString() == "true")
        {
            MouseSwitch.IsChecked = true;
        }
        key.Close();
        
        
    }

    private void LightMouse_white_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("LightMouse", "Light");
        key.Close();
        LightMouse_white.IsChecked = true;
        LightMouse_black.IsChecked = false;
    }

    private void LightMouse_black_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("LightMouse", "Dark");
        key.Close();
        LightMouse_black.IsChecked = true;
        LightMouse_white.IsChecked = false;
    }

    private void DarkMouse_white_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("DarkMouse", "Light");
        key.Close();
        DarkMouse_white.IsChecked = true;
        DarkMouse_black.IsChecked = false;
    }

    private void DarkMouse_black_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("DarkMouse", "Dark");
        key.Close();
        DarkMouse_black.IsChecked = true;
        DarkMouse_white.IsChecked = false;
    }


    private void MouseSwitch_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if (key.GetValue("SwitchMouse").ToString() == "true")
        {
            MouseSwitch.IsChecked = false;
            key.SetValue("SwitchMouse", "false");
        }
        else
        {
            MouseSwitch.IsChecked = true;
            key.SetValue("SwitchMouse", "true");
        }
        key.Close();
    }
}
