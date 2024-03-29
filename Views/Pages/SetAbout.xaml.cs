﻿using DarkMode_2.Models;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
    private async void CheckUpdate_onClick(object sender, System.Windows.RoutedEventArgs e)
    {
        NewVersion update = new NewVersion();
        ProgressRing.Visibility = System.Windows.Visibility.Visible;
        checkUpdateBtn.IsEnabled= false;
        

        string res = await update.CheckUpdate();
        Match match = Regex.Match(res, @"\d+\.\d+\.\d+\.\d+");
        if (match.Success)
        {
            DownloadWindow window = new DownloadWindow(match.Groups[1].Value);
            ProgressRing.Visibility = System.Windows.Visibility.Hidden;
            checkUpdateBtn.IsEnabled = true;
            window.ShowDialog();
        }
        else
        {
            OpenSnackbar(res);
            ProgressRing.Visibility = System.Windows.Visibility.Hidden;
            checkUpdateBtn.IsEnabled = true;
        }

    }

    private void VersionChannel_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenSnackbar(LanguageHandler.GetLocalizedString("SetAboutPage_Tip1"));
    }

    private void OpenSnackbar(string connect)
    {
        PlaySound(@"C:\Windows\Media\Windows Notify System Generic.wav", 0, 1);
        _snackbarService.Show(LanguageHandler.GetLocalizedString("SetAboutPage_Tip2"), connect, SymbolRegular.Alert24);
    }
}
