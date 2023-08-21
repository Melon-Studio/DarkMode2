using DarkMode_2.Models;
using DarkMode_2.Models.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

        private string url;
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

        private async void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NewVersion newVersion = new NewVersion();
            confirm.IsEnabled = false;
            version_name.Text = LanguageHandler.GetLocalizedString("DownloadWindow_Title") + _version;
            IUpdate.Channel channel = newVersion.UpdateChannel();
            string content = await newVersion.UpdateJsonInterpreter(null, IUpdate.type.Content, channel);


            string res = await newVersion.GetJson(channel);
            url = await newVersion.UpdateJsonInterpreter(res, IUpdate.type.DownloadUrl, channel);
            confirm.IsEnabled = true;
            load_progress.Visibility = Visibility.Collapsed;
            update_content.Visibility = Visibility.Visible;
            update_content.Text = content;
        }
    }
}
