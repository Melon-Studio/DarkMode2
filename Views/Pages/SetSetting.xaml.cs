using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using DarkMOde_2.Services.Contracts;
using log4net;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
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
    private static readonly ILog log = LogManager.GetLogger(typeof(SetSetting));

    private readonly ITestWindowService _testWindowService;

    private readonly IThemeService _themeService;

    private readonly ISnackbarService _snackbarService;
    private string ExceptionContent;

    private LanguageSettings _settings;

    private LanguageHandler _languageHandler;

    [DllImport("winmm.dll")]
    public static extern bool PlaySound(String Filename, int Mod, int Flags);

    public SetSetting(ISnackbarService snackbarService, IThemeService themeServices, ITestWindowService testWindowService, SetSettingViewModel viewModel)
    {
        InitializeComponent();
        _snackbarService = snackbarService;
        _testWindowService = testWindowService;
        _themeService = themeServices;
        _languageHandler = new LanguageHandler(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "i18n"));
        BingData();
        

        //设置初始化
        RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //开机自启
        if (AutoStartManager.IsAutoStartEnabled())
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
        //自动更新日出日落时间
        if (appkey.GetValue("SunRiseSet").ToString() == "false")
        {
            AutoUpdateTime.IsEnabled = false;
        }
        if (appkey.GetValue("AutoUpdateTime").ToString() == "true")
        {
            AutoUpdateTime.IsChecked = true;
        }
        //托盘栏图标
        if (appkey.GetValue("TrayBar").ToString() == "true")
        {
            TrayBar.IsChecked = true;
        }
        //语言
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "i18n", "languages_list.json");
        string jsonContent = File.ReadAllText(jsonFilePath);
        _settings = JsonConvert.DeserializeObject<LanguageSettings>(jsonContent);

        foreach (var language in _settings.Languages)
        {
            languageCombox.Items.Add(language.Name);
        }
        string savedLanguageCode = RegistryInit.GetSavedLanguageCode();
        languageCombox.SelectedItem = _settings.GetLanguageNameByCode(savedLanguageCode);
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

    //国际化语言

    private void languageCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        string selectedLanguageCode = _settings.GetLanguageCodeByName(languageCombox.SelectedItem.ToString());
        RegistryInit.SaveLanguageCode(selectedLanguageCode);
        _languageHandler.ChangeLanguage(RegistryInit.GetSavedLanguageCode());
        Refresh();
    }

    private void BingData()
    {
        Dictionary<int, string> data1 = new Dictionary<int, string>();
        data1.Add(0, LanguageHandler.GetLocalizedString("SetSettingPage_Tip1"));
        data1.Add(1, LanguageHandler.GetLocalizedString("SetSettingPage_Tip2"));
        data1.Add(2, LanguageHandler.GetLocalizedString("SetSettingPage_Tip3"));

        Dictionary<int, string> data2 = new Dictionary<int, string>();
        data2.Add(0, LanguageHandler.GetLocalizedString("SetSettingPage_Tip4"));
        data2.Add(1, LanguageHandler.GetLocalizedString("SetSettingPage_Tip5"));
        data2.Add(2, LanguageHandler.GetLocalizedString("SetSettingPage_Tip6"));

        ColorCombo.ItemsSource = data1;
        UpdateCombo.ItemsSource= data2;
        
    }
    //开机自启
    private void Autostart_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        if (AutoStartManager.IsAutoStartEnabled())
        {
            // 如果已经启用了开机自启，则禁用它
            AutoStartManager.DisableAutoStart();
            key.SetValue("DarkMode2", "false");
        }
        else
        {
            // 如果没有启用开机自启，则启用它
            AutoStartManager.EnableAutoStart();
            key.SetValue("DarkMode2", "true");
        }
        key.Close();
    }

    //自动更新
    private void AutoUpdate_onClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("AutoUpdate").ToString();
        if(state == "true")
        {
            key.SetValue("AutoUpdate", "false");
            key.Close();
        }else if(state == "false")
        {
            key.SetValue("AutoUpdate", "true");
            key.Close();
        }

    }
    //自动更新日出日落时间
    private void AutoUpdateTime_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("AutoUpdateTime").ToString();
        if (state == "true")
        {
            try
            {
                key.SetValue("AutoUpdateTime", "false");
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
            }
        }
        else if (state == "false")
        {
            try
            {
                key.SetValue("AutoUpdateTime", "true");
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
            }
        }
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
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
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
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
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
                OpenSnackbar(LanguageHandler.GetLocalizedString("SetSettingPage_Tip9"));
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
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
                OpenSnackbar(LanguageHandler.GetLocalizedString("SetSettingPage_Tip10"));
            }
            catch (Exception ex)
            {
                MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("SetSettingPage_Tip8"), ex.ToString());
                ExceptionContent = ex.ToString();
            }
        }
    }

    private void OpenSnackbar(string connect)
    {
        PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
        _snackbarService.Show(LanguageHandler.GetLocalizedString("SetSettingPage_Tip11"), connect, SymbolRegular.Alert24);
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
        //注册表重置
        RegistryInit.RegistryReset();
    }

    public void Refresh()
    {
        DispatcherFrame frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            new DispatcherOperationCallback(delegate (object f)
            {
                ((DispatcherFrame)f).Continue = false;
                return null;
            }
                ), frame);
        Dispatcher.PushFrame(frame);
    }

    
}
