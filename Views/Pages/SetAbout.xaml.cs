using DarkMode_2.Models;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetAbout.xaml 的交互逻辑
/// </summary>
public partial class SetAbout
{

    [DllImport("winmm.dll")]
    public static extern bool PlaySound(String Filename, int Mod, int Flags);
    private readonly ISnackbarService _snackbarService;
    public SetAbout(ISnackbarService snackbarService)
    {
        InitializeComponent();
        _snackbarService = snackbarService;
        Channel.Text = VersionControl.Channel();
        Version.Text = VersionControl.Version() + "(" + VersionControl.InternalVersion() + ")";
    }

    //帮助中心
    private void OpenDiscussions_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start("https://github.com/Melon-Studio/DarkMode2/discussions");
    }
    //检查更新
    private void CheckUpdate_onClick(object sender, System.Windows.RoutedEventArgs e)
    {
        Update update = new Update();
        ProgressRing.Visibility = System.Windows.Visibility.Visible;
        _ = Task.Run(() =>
        {
            string x = update.CheckUpdate();
            Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    OpenSnackbar(x);
                    ProgressRing.Visibility = System.Windows.Visibility.Hidden;
                })
            );
        });
        
    }

    private void VersionChannel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenSnackbar("该功能暂未开放");
    }

    private void OpenSnackbar(string connect)
    {
        PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
        _snackbarService.Show("提示", connect, SymbolRegular.Alert24);
    }

    private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StartUrl.StartUrlLink("https://licenses.nuget.org/Apache-2.0");
    }

    private void TextBlock_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StartUrl.StartUrlLink("https://licenses.nuget.org/MIT");
    }

    private void TextBlock_MouseDown_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StartUrl.StartUrlLink("https://licenses.nuget.org/MPL-2.0");
    }

    private void TextBlock_MouseDown_3(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StartUrl.StartUrlLink("https://licenses.nuget.org/Apache-2.0");
    }

    private void TextBlock_MouseDown_4(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        StartUrl.StartUrlLink("https://licenses.nuget.org/MIT");
    }
}
