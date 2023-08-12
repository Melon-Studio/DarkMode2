using DarkMode_2.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace DarkMode_2.Views
{
    /// <summary>
    /// DownloadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWindow
    {
        public Frame RootFrame { get; private set; }
        public INavigation RootNavigation { get; private set; }

        private string _version;
        public DownloadWindow(string version)
        {
            _version = version;
            InitializeComponent();
        }

        bool status = false;

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                DownloadManager download = new DownloadManager();
                download.ProgressChanged += DownloadManager_ProgressChanged;
                download.SpeedChanged += DownloadManager_SpeedChanged;
                download.DownloadCompleted += DownloadManager_DownloadCompleted;

                progressBar.Visibility = Visibility.Visible;
                speed.Visibility = Visibility.Visible;

                string url = "https://gitee.com/melon-studio/DarkMode2/releases/download/2.1.2.20230809-Release/DarkMode_Release_2.1.2.20230809.zip";
                await download.DownloadFileAsync(url);
            }
            else
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string targetExePath = Path.Combine(currentDirectory, "DarkModeUpdate.exe");
                Process.Start(targetExePath);
                Application.Current.Shutdown();
            }
            
        }

        private void DownloadManager_DownloadCompleted(string obj)
        {
            status = true;
            confirm.Content = LanguageHandler.GetLocalizedString("DownloadWindow_Install");
            speed.Visibility = Visibility.Hidden;
        }

        private void DownloadManager_SpeedChanged(double obj)
        {
            Dispatcher.Invoke(() =>
            {
                if (obj < 1024)
                {
                    double speedInKBps = obj * 1024;
                    speed.Text = $"{speedInKBps:F2} KB/s";
                }
                else
                {
                    double speedInMBps = obj / 1024;
                    speed.Text = $"{speedInMBps:F2} MB/s";
                }
            });
        }

        private void DownloadManager_ProgressChanged(double obj)
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Value = obj * 100;
            });
        }

        private async void UiWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            version_name.Text = LanguageHandler.GetLocalizedString("DownloadWindow_Title") + _version;
            string content = await new Update().UpdateContent(_version);
            load_progress.Visibility = System.Windows.Visibility.Collapsed;
            update_content.Visibility = System.Windows.Visibility.Visible;
            update_content.Text = content;
        }
    }
}
