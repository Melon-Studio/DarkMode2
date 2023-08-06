using DarkMode_2.Services;
using DarkMode_2.ViewModels;
using DarkMOde_2.Services;
using DarkMOde_2.Services.Contracts;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace DarkMode_2;

/// <summary>
/// App.xaml 的交互逻辑
/// </summary>
public partial class App
{
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
        .ConfigureServices((context, services) =>
        {
            // 注册程序实例
            services.AddHostedService<ApplicationHostService>();

            // 注册主题实例
            services.AddSingleton<IThemeService, ThemeService>();

            // 注册任务栏操作实例
            services.AddSingleton<ITaskBarService, TaskBarService>();

            // 注册弹窗实例
            services.AddSingleton<ISnackbarService, SnackbarService>();

            // 注册命令行参数实例
            //services.AddSingleton<IEnvironmentService, Services.Contracts.MyProductionEnvironmentService>();

            // 注册对话服务实例
            services.AddSingleton<IDialogService, DialogService>();

            // 窗口页面解析
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<ITestWindowService, TestWindowService>();

            // 注册Navigation实例
            services.AddSingleton<INavigationService, NavigationService>();

            // 注册启动窗口实例
            services.AddScoped<INavigationWindow, Views.MainWindow>();
            services.AddScoped<MainWindowViewModel>();

            // 注册Page页面实例
            services.AddScoped<Views.Pages.SetTimes>();
            services.AddScoped<Views.Pages.SetAbout>();
            services.AddScoped<Views.Pages.SetDIY>();
            services.AddScoped<Views.Pages.SetMore>();
            services.AddScoped<Views.Pages.SetSetting>();
            services.AddScoped<Views.Pages.SetWallpaper>();

            services.AddScoped<SetTimesViewModel>();
            services.AddScoped<SetAboutViewModel>();
            services.AddScoped<SetDIYViewModel>();
            services.AddScoped<SetMoreViewModel>();
            services.AddScoped<SetSettingViewModel>();
            services.AddScoped<SetWallpaperViewModel>();

            // 注册窗口及模型实例
            services.AddTransient<Views.SettingsWindow>();
            services.AddTransient<SettingsViewModel>();

            services.AddTransient<Views.DeveloperModeWindow>();
            services.AddTransient<DeveloperModeViewModel>();
        }).Build();

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        XmlConfigurator.Configure();
        string logDirectory = Path.Combine(Path.GetTempPath(), "DarkMode2", "logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        //启动程序入口
        await _host.StartAsync();
    }
}
