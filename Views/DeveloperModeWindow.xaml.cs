using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using Microsoft.Win32;
//using OpenHardwareMonitor.Hardware;
using System;
using System.Collections;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;
using System.Windows;
using Windows.Devices.Sensors;
using MessageBox = DarkMode_2.Models.MessageBox;

namespace DarkMode_2.Views;

/// <summary>
/// DeveloperModeWindow.xaml 的交互逻辑
/// </summary>
public partial class DeveloperModeWindow
{
    string serviceFilePath = "E:\\Project\\C#\\DarkModeService\\bin\\Debug\\DarkModeService.exe";
    string serviceName = "DarkMode Service";
    public DeveloperModeViewModel ViewModel
    {
        get;
    }

    private LightSensor _lightSensor;
    private Timer _timer;
    public DeveloperModeWindow(DeveloperModeViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        _lightSensor = LightSensor.GetDefault();
        if (_lightSensor != null)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }
        else
        {
            Console.WriteLine("你的设备不存在感光元件，无法使用此功能。");
        }

        //GPUload.Text = GetGPULoad.GetUsage().ToString();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        var lightReading = _lightSensor.GetCurrentReading();
        Console.WriteLine("感光度: {0} lux", lightReading.IlluminanceInLux);
    }
    private void UiWindow_Loaded(object sender, RoutedEventArgs e)
    {
        
    }
    private void Internal_OnClick(object sender, RoutedEventArgs e)
    {

    }
    private void Dialog_OnClick(object sender, RoutedEventArgs e)
    {

    }
    private void Message_OnClick(object sender, RoutedEventArgs e)
    {
        OpenMessageBox("错误发生", "你猜发生了什么错误？");
    }

    private void OpenMessageBox(string title, string content)
    {
        var messageBox = new Wpf.Ui.Controls.MessageBox();

        messageBox.ButtonLeftName = "复制内容";
        messageBox.ButtonRightName = "关闭弹窗";

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
        messageBox.ButtonRightClick += MessageBox_RightButtonClick;

        messageBox.Show(title, content);
    }

    private void MessageBox_LeftButtonClick(object sender, RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }

    private void MessageBox_RightButtonClick(object sender, RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }

    //发送消息
    //声明 API 函数
    [DllImport("User32.dll", EntryPoint = "SendMessage")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    //定义消息常数
    public const int CUSTOM_MESSAGE = 0X000F;//自定义消息


    

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // TUDO:TEST
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        WallpaperChanger wallpaperChanger = new(key.GetValue("WeInstallPath").ToString(), key.GetValue("WeDark").ToString());
        wallpaperChanger.openWallpaper();
    }
    
    private void InstallService_Click(object sender, RoutedEventArgs e)
    {
        using (AssemblyInstaller installer = new AssemblyInstaller())
        {
            installer.UseNewContext = true;
            installer.Path = serviceFilePath;
            IDictionary savedState = new Hashtable();
            installer.Install(savedState);
            installer.Commit(savedState);
            MessageBox.OpenMessageBox("提示","服务安装成功。");
        }
    }
    private void UnInstallService_Click(object sender, RoutedEventArgs e)
    {
        using (AssemblyInstaller installer = new AssemblyInstaller())
        {
            installer.UseNewContext = true;
            installer.Path = serviceFilePath;
            installer.Uninstall(null);
            MessageBox.OpenMessageBox("提示", "服务卸载成功。");
        }
    }
    private void StartService_Click(object sender, RoutedEventArgs e)
    {
        using (ServiceController control = new ServiceController(serviceName))
        {
            if (control.Status == ServiceControllerStatus.Stopped)
            {
                control.Start();
                MessageBox.OpenMessageBox("提示", "服务启动成功。");
            }
        }
    }
}

