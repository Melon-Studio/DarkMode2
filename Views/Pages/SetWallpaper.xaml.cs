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
        dialog.Title = "请选择壁纸图片";
        dialog.Filter = "图片文件|*.jpg;*.jpeg;*.bmp;*.dib;*.png;*.jfif;*.jpe;*.gif;*.tif;*.tiff;*.wdp;*.heic;*.heif;*.heics;*.heifs;*.hif;*.avci;*.avcs;*.avif;*.avifs";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LightBox1.Text = dialog.FileName;
        }
    }

    private void BrowseButton2_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = "请选择壁纸图片";
        dialog.Filter = "图片文件|*.jpg;*.jpeg;*.bmp;*.dib;*.png;*.jfif;*.jpe;*.gif;*.tif;*.tiff;*.wdp;*.heic;*.heif;*.heics;*.heifs;*.hif;*.avci;*.avcs;*.avif;*.avifs";
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
        dialog.Title = "请选择JSON文件";
        dialog.Filter = "json文件|*.json;";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LightBox2.Text = dialog.FileName;
        }
    }

    private void BrowseButton4_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = "请选择JSON文件";
        dialog.Filter = "json文件|*.json;";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            DarkBox2.Text = dialog.FileName;
        }
    }

    private void Save2_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("WeLight", "\""+LightBox2.Text+ "\"");
        key.SetValue("WeDark", "\""+DarkBox2.Text+ "\"");
        key.SetValue("WeInstallPath", "\""+WePath.Text+ "\"");
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
                installPath64 = Regex.Match(installPath32, @"((?!\\wallpaper32.exe).)*").ToString() + @"\wallpaper64.exe";
                WePath.Text = installPath64;
            }
        }
        catch
        {
            OpenSnackbar("提示","自动搜索路径失败，请手动选择。");
        }
    }

    private void ManualButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        dialog.Multiselect = false;
        dialog.Title = "请选择 wallpaper64.exe 文件";
        dialog.Filter = "exe文件|wallpaper64.exe;";
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
