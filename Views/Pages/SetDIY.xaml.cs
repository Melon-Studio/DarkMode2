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
        
    }

    private void LightMouse_white_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        LightMouse_white.IsChecked = true;
        LightMouse_black.IsChecked = false;
    }

    private void LightMouse_black_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        LightMouse_black.IsChecked = true;
        LightMouse_white.IsChecked = false;
    }

    private void DarkMouse_white_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DarkMouse_white.IsChecked = true;
        DarkMouse_black.IsChecked = false;
    }

    private void DarkMouse_black_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DarkMouse_black.IsChecked = true;
        DarkMouse_white.IsChecked = false;
    }
}
