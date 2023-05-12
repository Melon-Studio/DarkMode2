using DarkMode_2.ViewModels;
using DarkMOde_2.Services.Contracts;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using MessageBox = DarkMode_2.Models.MessageBox;
using MessageBox2 = System.Windows.Forms.MessageBox;
using System.Windows.Input;
using NHotkey.Wpf;
using NHotkey;
using System.Linq;
using System.Threading;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Services;
using System.Diagnostics;
using DarkMode_2.Models;
using System.IO;
using System.Net;
using System.Text;

namespace DarkMode_2.Views;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : INavigationWindow
{

    private readonly ITestWindowService _testWindowService;

    private readonly IThemeService _themeService;

    private readonly ITaskBarService _taskBarService;

    public MainWindowViewModel ViewModel
    {
        get;
    }
    public Frame RootFrame { get; private set; }
    public INavigation RootNavigation { get; private set; }

    public MainWindow(MainWindowViewModel viewModel, IPageService pageService, ITaskBarService taskBarService, ITestWindowService testWindowService, IThemeService themeServices)
    {
        ViewModel = viewModel;
        DataContext = this;
        _taskBarService = taskBarService;
        _testWindowService = testWindowService;
        _themeService = themeServices;
        InitializeComponent();
        // 判断是否为支持的操作系统
        string WinVersion = WindowsVersionHelper.GetWindowsEdition();
        if(WinVersion != "Windows 10" && WinVersion != "Windows 11")
        {
            MessageBox2.Show($"抱歉，本程序仅支持Windows10/11的操作系统。\n你的操作系统版本是：{WinVersion}", "无法提供服务");
            Application.Current.Shutdown();
        }
        try
        {
            HotkeyManager.Current.AddOrReplace("Start", Key.D, ModifierKeys.Control | ModifierKeys.Alt, OnStart);
        }
        catch
        {
            MessageBox.OpenMessageBox("DarkMode 错误：快捷键被占用", "Ctrl+Alt+D 快捷键被占用，将无法通过快捷键打开设置，请勿在设置中关闭托盘栏图标，如果意外关闭，请进入 DarkMode 的 GitHub 仓库的 Discussions 页面查看帮助。");
        }

        SetPageService(pageService);

        //注册表初始化
        try
        {
            RegistryKey pan;
            RegistryKey key;
            pan = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2");

            if (pan == null)
            {
                key = Registry.CurrentUser.CreateSubKey(@"Software\DarkMode2");
                key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
                //软件安装路径
                key.SetValue("DarkModeInstallPath", System.Windows.Forms.Application.ExecutablePath);
                //开机自启
                key.SetValue("DarkMode2", "false");
                //系统颜色初始化
                key.SetValue("IsLight", "false");
                //浅色模式开始时间
                key.SetValue("startTime", "08:00");
                //浅色模式结束时间
                key.SetValue("endTime", "19:00");
                //软件语言
                key.SetValue("Language", "zh-CN");
                //日出日落模式
                key.SetValue("SunRiseSet", "false");
                //自动更新日出日落时间
                key.SetValue("AutoUpdateTime", "false");
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

                // 收集信息
                string url = "https://api.dooper.top/darkmode/API/UsersCollect.php";
                string windowsEdtion = WindowsVersionHelper.GetWindowsEdition();
                string windowsVersion = WindowsVersionHelper.GetWindowsVersion().ToString();
                string channel = VersionControl.Channel();
                string version = VersionControl.Version();
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string data = $"WindowsEdition={windowsEdtion}&WindowsVersion={windowsVersion}&Channel={channel}&Version={version}&Date={date}"; // 拼接成x-www-form-urlencoded格式的字符串
                string result = SendPostRequest(url, data);
                Console.WriteLine(result);
            }
        }
        catch (Exception ex)
        {
            MessageBox.OpenMessageBox("错误发生", ex.ToString());
        }
        RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        //设置初始化
        if (appkey.GetValue("TrayBar").ToString() == "false")
        {
            HideTrayBarIcon();
        }
        appkey.Close();
        //主题跟随系统定时器
        var timerGetTime = new System.Windows.Forms.Timer();
        //设置定时器属性
        timerGetTime.Tick += new EventHandler(SwitchService);
        timerGetTime.Interval = 100;
        timerGetTime.Enabled = true;
        //开启定时器
        timerGetTime.Start();
    }

    public static string SendPostRequest(string url, string data)
    {
        // 创建一个请求对象
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Timeout = 3000;

        // 将要发送的数据转换为字节数组
        byte[] byteData = Encoding.UTF8.GetBytes(data);

        // 设置请求的内容长度
        request.ContentLength = byteData.Length;

        // 获取请求的输出流对象，可以向这个流对象写入请求的数据
        Stream outputStream = request.GetRequestStream();
        outputStream.Write(byteData, 0, byteData.Length);
        outputStream.Close();

        // 发送请求并获取响应
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
        string result = reader.ReadToEnd();
        reader.Close();
        responseStream.Close();

        return result;
    }
    public void SwitchService(Object myObject, EventArgs myEventArgs)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //判断是否在浅色时间段内
        string startLightTime = key.GetValue("startTime").ToString();
        string endLightTime = key.GetValue("endTime").ToString();
        TimeSpan _startLightTime = DateTime.Parse(startLightTime).TimeOfDay;
        TimeSpan _endLightTime = DateTime.Parse(endLightTime).TimeOfDay;

        DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("t"));
        TimeSpan dspNow = dateTime.TimeOfDay;
        if (dspNow > _startLightTime && dspNow < _endLightTime)
        {
            //在时间段内
            SwitchMode.switchMode("light");
            //Console.WriteLine("切换为浅色");
        }
        else
        {
            //不在时间段内
            SwitchMode.switchMode("dark");
            //Console.WriteLine("切换为深色");
        }

    }

    private void OnStart(object sender, HotkeyEventArgs e)
    {
        bool isWindowOpen = false;
        foreach (Window w in Application.Current.Windows)
        {
            if (w is SettingsWindow)
            {
                isWindowOpen = true;
                w.Activate();
            }
        }
        if (!isWindowOpen)
        {
            _testWindowService.Show<SettingsWindow>();
        }
    }

    private void HideTrayBarIcon()
    {
        NotifyIcon.Visibility = Visibility.Collapsed;
    }

    public Frame GetFrame()
        => RootFrame;

    public INavigation GetNavigation()
        => RootNavigation;

    public bool Navigate(Type pageType)
        => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) { }

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //隐藏窗口
        this.Hide();
        //消息通知
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        if (key.GetValue("Notification").ToString() == "true")
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("DarkMode 通知")
                .AddText("DarkMode 运行中，配置请点击DarkMode图标")
                .Show();
        }
        key.Close();
    }

    private void Start_OnClick(object sender, RoutedEventArgs e)
    {
        //打开SettingsWindow窗口
        bool isWindowOpen = false;
        foreach (Window w in Application.Current.Windows)
        {
            if (w is SettingsWindow)
            {
                isWindowOpen = true;
                w.Activate();
            }
        }
        if (!isWindowOpen)
        {
            _testWindowService.Show<SettingsWindow>();
        }
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        //退出程序
        Application.Current.Shutdown();
    }
}
