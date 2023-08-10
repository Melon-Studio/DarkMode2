using DarkMode_2.Models;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetWallpaper.xaml 的交互逻辑
/// </summary>
public partial class SetWallpaper
{
    private readonly ISnackbarService _snackbarService;
    public SetWallpaper(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        InitializeComponent();

        //设置初始化
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        LightBox1.Text = key.GetValue("NativeLight").ToString();
        DarkBox1.Text = key.GetValue("NativeDark").ToString();
        LightBox2.Text = key.GetValue("WeLight").ToString();
        DarkBox2.Text = key.GetValue("WeDark").ToString();
        WePath.Text = key.GetValue("WeInstallPath").ToString();
        key.Close();
        
    }

    private void BrowseButton1_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip1");
        dialog.Filter = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip2");
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LightBox1.Text = dialog.FileName;
        }
    }

    private void BrowseButton2_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip1");
        dialog.Filter = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip2");
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            DarkBox1.Text = dialog.FileName;
        }
    }

    private void Save1_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("NativeLight",LightBox1.Text);
        key.SetValue("NativeDark", DarkBox1.Text);
        key.Close();
    }

    private void BrowseButton3_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip3");
        dialog.Filter = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip4");
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LightBox2.Text = dialog.FileName;
        }
    }

    private void BrowseButton4_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip3");
        dialog.Filter = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip4");
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            DarkBox2.Text = dialog.FileName;
        }
    }

    private void Save2_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("WeLight", LightBox2.Text);
        key.SetValue("WeDark", DarkBox2.Text);
        key.SetValue("WeInstallPath", WePath.Text);
        key.Close();
    }

    private void AutoButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\WallpaperEngine", false);
        try
        {
            string installPath32 = key.GetValue("installPath").ToString();
            string installPath64;
            if(installPath32 != "")
            {
                installPath64 = Regex.Match(installPath32, @"((?!\\wallpaper32.exe).)*").ToString();
                WePath.Text = installPath64 + "\\wallpaper64.exe";
            }
        }
        catch
        {
            OpenSnackbar(LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip5"), LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip6"));
        }
    }

    private void ManualButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip7");
        dialog.Filter = LanguageHandler.GetLocalizedString("SetWallpaperPage_Tip8s");
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            WePath.Text = dialog.FileName;
        }
    }

    private void OpenSnackbar(string title, string connect)
    {
        _snackbarService.Show(title, connect, SymbolRegular.FoodCake24);
    }
}
