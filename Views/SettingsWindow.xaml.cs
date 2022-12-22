using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using DarkMode_2.ViewModels;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.TaskBar;
using Microsoft.Win32;
using Wpf.Ui.Appearance;
using DarkMode_2.Models;

namespace DarkMode_2.Views;

/// <summary>
/// SettingsWindow.xaml 的交互逻辑
/// </summary>
public partial class SettingsWindow : INavigationWindow
{
    private bool _initialized = false;

    private readonly IThemeService _themeService;

    private readonly ITaskBarService _taskBarService;


    public SettingsViewModel ViewModel
    {
        get;
    }
    public SettingsWindow(SettingsViewModel viewModel, IThemeService themeServices, INavigationService navigationService, IPageService pageService, IThemeService themeService, ITaskBarService taskBarService, ISnackbarService snackbarService, IDialogService dialogService)
    {
        ViewModel = viewModel;
        DataContext = this;
        _themeService = themeServices;
        _taskBarService = taskBarService;
        InitializeComponent();
        SetPageService(pageService);
        navigationService.SetNavigationControl(RootNavigation);
        snackbarService.SetSnackbarControl(RootSnackbar);
        dialogService.SetDialogControl(RootDialog);

        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        if (key.GetValue("ColorMode").ToString() == "Light")
        {
            _themeService.SetTheme(ThemeType.Light);
        }
        else if (key.GetValue("ColorMode").ToString() == "Dark")
        {
            _themeService.SetTheme(ThemeType.Dark);
        }else if (key.GetValue("ColorMode").ToString() == "Auto")
        {
            if(DetermineSystemColorMode.GetState() == "light")
            {
                _themeService.SetTheme(ThemeType.Light);
            }
            else if(DetermineSystemColorMode.GetState() == "dark")
            {
                _themeService.SetTheme(ThemeType.Dark);
            }
        }

        //主题跟随系统定时器
        var timerGetTime = new System.Windows.Forms.Timer();
        //设置定时器属性
        timerGetTime.Tick += new EventHandler(ChangeMonitor);
        timerGetTime.Interval = 100;
        timerGetTime.Enabled = true;
        //开启定时器
        timerGetTime.Start();
    }

    public void ChangeMonitor(Object myObject, EventArgs myEventArgs)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        if (key.GetValue("ColorMode").ToString() == "Auto")
        {
            if (DetermineSystemColorMode.GetState() == "dark")
            {
                _themeService.SetTheme(ThemeType.Dark);
            }
            else if (DetermineSystemColorMode.GetState() == "light")
            {
                _themeService.SetTheme(ThemeType.Light);
            }
        }
        key.Close();
    }
    public Frame GetFrame()
        => RootFrame;

    public INavigation GetNavigation()
        => RootNavigation;

    public bool Navigate(Type pageType)
        => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService)
        => RootNavigation.PageService = pageService;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();


    private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
        RootFrame.Margin = new Thickness(
            left: 0,
            top: sender?.Current?.PageTag == "settimes" ? -69 : 0,
            right: 0,
            bottom: 0);
    }

    private void UiWindow_Loaded(object sender, RoutedEventArgs e)
    {
        /// <summary>
        /// 打开启动窗口的界面
        /// </summary>
        if (_initialized)
            return;

        _initialized = true;

        RootMainGrid.Visibility = Visibility.Collapsed;

        _taskBarService.SetState(this, TaskBarProgressState.Indeterminate);

        Task.Run(async () =>
        {
            await Task.Delay(100);

            await Dispatcher.InvokeAsync(() =>
            {
                RootMainGrid.Visibility = Visibility.Visible;

                Navigate(typeof(Pages.SetTimes)); //页面

                _taskBarService.SetState(this, TaskBarProgressState.None);
            });
        });
    }

}
