using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using LibreHardwareMonitor.Hardware;
//using OpenHardwareMonitor.Hardware;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace DarkMode_2.Views;

/// <summary>
/// DeveloperModeWindow.xaml 的交互逻辑
/// </summary>
public partial class DeveloperModeWindow
{
    public DeveloperModeViewModel ViewModel
    {
        get;
    }
    public DeveloperModeWindow(DeveloperModeViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        //GPUload.Text = GetGPULoad.GetUsage().ToString();
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


    //向窗体发送消息的函数
    public static void SendMsgToTaskbar()
    {
        IntPtr WINDOW_HANDLER = FindWindow(null, "explorer");
        if (WINDOW_HANDLER == IntPtr.Zero)
        {
            throw new Exception("Could not find explorer!");
        }
        else
        {
            SendMessage(WINDOW_HANDLER, CUSTOM_MESSAGE, new IntPtr(14), IntPtr.Zero).ToInt64();
            //IntPtr WINDOW_TASKBAR = FindWindowEx(WINDOW_HANDLER, IntPtr.Zero, null, "DesktopWindowXamlSource");
            //if (WINDOW_TASKBAR == IntPtr.Zero)
            //{
            //    throw new Exception("Could not find Taskbar!");
            //}
        }
        //SendMessage(WINDOW_HANDLER, CUSTOM_MESSAGE, new IntPtr(14), IntPtr.Zero).ToInt64();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        SendMsgToTaskbar();
    }
}

