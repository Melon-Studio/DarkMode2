using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using DarkMOde_2.Services.Contracts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = DarkMode_2.Models.MessageBox;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetSetting.xaml 的交互逻辑
/// </summary>
public partial class SetSetting 
{

    private readonly ITestWindowService _testWindowService;

    private readonly IThemeService _themeService;

    private readonly ISnackbarService _snackbarService;

    [DllImport("winmm.dll")]
    public static extern bool PlaySound(String Filename, int Mod, int Flags);

    public SetSetting(ISnackbarService snackbarService, IThemeService themeServices, ITestWindowService testWindowService, SetSettingViewModel viewModel)
    {
        InitializeComponent();
        _snackbarService = snackbarService;
        _testWindowService = testWindowService;
        _themeService = themeServices;
        BingData();
        

        //设置初始化
        RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //开机自启
        if (appkey.GetValue("DarkMode2").ToString() == "true")
        {
            Autostart.IsChecked = true;
        }
        //消息通知
        if (appkey.GetValue("Notification").ToString() == "true")
        {
            Notification.IsChecked = true;
        }
        //自动更新
        if(appkey.GetValue("AutoUpdate").ToString() == "true")
        {
            AutoUpdate.IsChecked = true;
        }
        //托盘栏图标
        if (appkey.GetValue("TrayBar").ToString() == "true")
        {
            TrayBar.IsChecked = true;
        }
        //语言
        if (appkey.GetValue("Language").ToString() == "zh-CN")
        {
            languageCombo.SelectedIndex = 0;
        }
        //主题色
        if (appkey.GetValue("ColorMode").ToString() == "Auto")
        {
            ColorCombo.SelectedIndex = 0;
        }else if(appkey.GetValue("ColorMode").ToString() == "Light")
        {
            ColorCombo.SelectedIndex = 1;
        }
        else if (appkey.GetValue("ColorMode").ToString() == "Dark")
        {
            ColorCombo.SelectedIndex = 2;
        }
        //更新渠道
        if(appkey.GetValue("UpdateChannels").ToString() == "Auto")
        {
            UpdateCombo.SelectedIndex = 0;
        }else if(appkey.GetValue("UpdateChannels").ToString() == "Github")
        {
            UpdateCombo.SelectedIndex = 1;
        }else if(appkey.GetValue("UpdateChannels").ToString() == "Gitee")
        {
            UpdateCombo.SelectedIndex = 2;
        }
        appkey.Close();
        
    }

    private void BingData()
    {
        Dictionary<int,string> data = new Dictionary<int,string>();
        data.Add(0, "简体中文(zh-CN)");
        //data.Add(1, "繁體中文(zh-HK)");
        //data.Add(2, "English(en-US)");
        //data.Add(3, "Русский(ru-RU)");
        //data.Add(4, "日本語(ja-JP)");

        Dictionary<int, string> data1 = new Dictionary<int, string>();
        data1.Add(0, "跟随系统");
        data1.Add(1, "浅色");
        data1.Add(2, "深色");

        Dictionary<int, string> data2 = new Dictionary<int, string>();
        data2.Add(0, "自动");
        data2.Add(1, "GitHub渠道");
        data2.Add(2, "Gitee渠道");

        languageCombo.ItemsSource = data;
        ColorCombo.ItemsSource = data1;
        UpdateCombo.ItemsSource= data2;
        
    }
    //开机自启
    private void Autostart_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("DarkMode2").ToString();
        if (state == "false")
        {
            try
            {
                string path = Process.GetCurrentProcess().MainModule.FileName;
                RegistryKey syskey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2",true);
                syskey.SetValue("DarkMode2", path);
                appkey.SetValue("DarkMode2", "true");
                syskey.Close();
                appkey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
        else if (state == "true")
        {
            try
            {
                RegistryKey syskey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
                syskey.DeleteValue("DarkMode2", false);
                appkey.SetValue("DarkMode2", "false");
                syskey.Close();
                appkey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
    }

    //自动更新
    private void AutoUpdate_onClick(object sender, RoutedEventArgs e)
    {
        //RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //string state = key.GetValue("AutoUpdate").ToString();
        //if(state == "true")
        //{
        //    key.SetValue("AutoUpdate", "false");
        //    key.Close();
        //}else if(state == "false")
        //{
        //    key.SetValue("AutoUpdate", "true");
        //    key.Close();
        //}
        OpenSnackbar("该功能暂不可用");
        AutoUpdate.IsChecked = false;

    }

    //消息通知
    private void Notification_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("Notification").ToString();
        if (state == "true")
        {
            Notification.IsChecked = false;
            try
            {
                key.SetValue("Notification", "false");
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
        else if (state == "false")
        {
            try
            {
                key.SetValue("Notification", "true");
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
    }

    

    //托盘图标
    private void TrayBar_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("TrayBar").ToString();
        if (state == "true")
        {
            TrayBar.IsChecked = false;
            try
            {
                key.SetValue("TrayBar", "false");
                key.Close();
                PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
                OpenSnackbar("下次启动时生效，关闭托盘栏图标后可以使用 Ctrl+Alt+D 来快速打开设置中心");
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
        else if (state == "false")
        {
            TrayBar.IsChecked = true;
            try
            {
                key.SetValue("TrayBar", "true");
                key.Close();
                PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
                OpenSnackbar("下次启动时生效");
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox("错误发生", ex.ToString());
            }
        }
    }

    private void OpenSnackbar(string connect)
    {
        PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
        _snackbarService.Show("提示", connect, SymbolRegular.Alert24);
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    IntPtr hwnd = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
    private void DeveloperMode_Click(object sender, RoutedEventArgs e)
    {
        bool isWindowOpen = false;
        foreach (Window w in Application.Current.Windows)
        {
            if (w is DeveloperModeWindow)
            {
                isWindowOpen = true;
                w.Activate();
            }
        }
        if (!isWindowOpen)
        {
            _testWindowService.Show<DeveloperModeWindow>();
        }
    }

    private void UpdateCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if((int)UpdateCombo.SelectedValue == 0)
        {
            key.SetValue("UpdateChannels", "Auto");
        }else if((int)UpdateCombo.SelectedValue == 1)
        {
            key.SetValue("UpdateChannels", "Github");
        }else if((int)UpdateCombo.SelectedValue == 2)
        {
            key.SetValue("UpdateChannels", "Gitee");
        }
        key.Close();
    }

    private void ColorCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if ((int)ColorCombo.SelectedValue == 0)
        {
            key.SetValue("ColorMode", "Auto");
            if (DetermineSystemColorMode.GetState() == "light")
            {
                _themeService.SetTheme(ThemeType.Light);
            }
            else if (DetermineSystemColorMode.GetState() == "dark")
            {
                _themeService.SetTheme(ThemeType.Dark);
            }
        }
        else if ((int)ColorCombo.SelectedValue == 1)
        {
            key.SetValue("ColorMode", "Light");
            _themeService.SetTheme(ThemeType.Light);
        }
        else if ((int)ColorCombo.SelectedValue == 2)
        {
            key.SetValue("ColorMode", "Dark");
            _themeService.SetTheme(ThemeType.Dark);
        }
        key.Close();
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        //注册表初始化
        try
        {
            RegistryKey pan;
            RegistryKey key;
            pan = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2");
            key = Registry.CurrentUser.CreateSubKey(@"Software\DarkMode2");
            key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
            //开机自启
            key.SetValue("DarkMode2", "false");
            //浅色模式开始时间
            key.SetValue("startTime", "08:00");
            //浅色模式结束时间
            key.SetValue("endTime", "19:00");
            //软件语言
            key.SetValue("Language", "zh-CN");
            //日出日落模式
            key.SetValue("SunRiseSet", "false");
            //消息通知
            key.SetValue("Notification", "true");
            //托盘栏图标
            key.SetValue("TrayBar", "true");
            //软件主题色
            key.SetValue("ColorMode", "Auto");
            //软件自动更新
            key.SetValue("AutoUpdate", "false");
            //原生壁纸
            key.SetValue("NativeLight", "");
            key.SetValue("NativeDark", "");
            //Wallpaper Engine壁纸
            key.SetValue("WeLight", "");
            key.SetValue("WeDark", "");
            //更新渠道
            key.SetValue("UpdateChannels", "Auto");
            //鼠标主题
            key.SetValue("MouseMode", "Light");
            key.SetValue("LightMouse", "Light");
            key.SetValue("DarkMouse", "Light");
            //触摸键盘主题
            key.SetValue("KeyboardMode", "Light");
            //游戏模式
            key.SetValue("GameMode", "false");
            //Wallpaper Engine安装路径
            key.SetValue("WeInstallPath", "");

            key.Close();
            pan.Close();
            MessageBox.OpenMessageBox("重置成功", "用户配置文件已重置，请重启软件");
            
        }
        catch (Exception ex)
        {
            MessageBox.OpenMessageBox("错误发生", ex.ToString());
        }
    }
}
