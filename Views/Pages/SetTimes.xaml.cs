using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using System.Runtime.InteropServices;
using log4net;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetTimes.xaml 的交互逻辑
/// </summary>
public partial class SetTimes
{

    private static readonly ILog log = LogManager.GetLogger(typeof(SetTimes));
    private readonly ISnackbarService _snackbarService;

    private readonly IDialogControl _dialogControl;

    private const int MonitorBrightness = 0x0000000A;
    private const int BrightnessMinimum = 0;
    private const int BrightnessMaximum = 100;

    [DllImport("user32.dll")]
    private static extern bool GetMonitorBrightness(IntPtr hMonitor, out uint pdwMinimumBrightness, out uint pdwCurrentBrightness, out uint pdwMaximumBrightness);

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    public SetTimes(ISnackbarService snackbarService, IDialogService dialogService, SetTimesViewModel viewModel)
    {
        InitializeComponent();

        _snackbarService = snackbarService;
        _dialogControl = dialogService.GetDialogControl();
        Loaded += OnLoaded;
        BingData();

        //设置初始化
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //设置时间
        UpdateTime();
        //设置日出日落模式
        if (key.GetValue("SunRiseSet").ToString() == "true")
        {
            SunRiseSet.IsChecked = true;

            setSunRise();

            SunRiseSwitch.IsExpanded = true;
            startTimeHours.IsEnabled = false;
            startTimeMinutes.IsEnabled = false;
            endTimeHours.IsEnabled = false;
            endTimeMinutes.IsEnabled = false;
        }
        else
        {
            SunRiseSet.IsChecked = false;
            SunRiseSwitch.IsExpanded = false;
        }
        // 感光模式
        if(key.GetValue("PhotosensitiveMode").ToString() == "true")
        {
            Autostart.IsChecked = true;
        }
        key.Close();
    }

    private void BingData()
    {
        Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
        for (int i = 0; i <= 24; i++)
        {
            keyValuePairs.Add(i, i.ToString("D2"));
        }

        startTimeHours.ItemsSource = keyValuePairs;
        endTimeHours.ItemsSource = keyValuePairs;
        startTimeHours.SelectedIndex = 0;
        endTimeHours.SelectedIndex = 0;

        Dictionary<int, string> keyValuePairs1 = new Dictionary<int, string>();
        for (int i = 0; i <= 60; i++)
        {
            keyValuePairs1.Add(i, i.ToString("D2"));
        }

        startTimeMinutes.ItemsSource = keyValuePairs1;
        endTimeMinutes.ItemsSource = keyValuePairs1;
        startTimeMinutes.SelectedIndex = 0;
        endTimeMinutes.SelectedIndex = 0;

    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;

        _dialogControl.ButtonRightClick += DialogControlOnButtonRightClick;
        _dialogControl.ButtonLeftClick += DialogControlOnButtonLeftClick;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _dialogControl.ButtonRightClick -= DialogControlOnButtonRightClick;
        _dialogControl.ButtonLeftClick -= DialogControlOnButtonLeftClick;
    }

    private void SunRiseSet_OnClick(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("SunRiseSet").ToString();
        
        if(state == "false" && SunRiseSet.IsChecked == true) 
        {
            OpenDialog(LanguageHandler.GetLocalizedString("SetTimesPage_Tip1"), LanguageHandler.GetLocalizedString("SetTimesPage_Tip2"));
        }
        if(SunRiseSet.IsChecked == false)
        {
            key.SetValue("SunRiseSet", "false");
            location.Text = "";
            lat.Text = "";
            lng.Text = "";
            SunRiseSwitch.IsExpanded = false;
            startTimeHours.IsEnabled= true;
            endTimeHours.IsEnabled= true;
            startTimeMinutes.IsEnabled= true;
            endTimeMinutes.IsEnabled= true;
        }
        key.Close();

    }

    private void OpenSnackbar(string title, string connect)
    {
        _snackbarService.Show(title, connect, SymbolRegular.Warning24);
    }

    private void UpdateTime()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string _startHours = Regex.Match(key.GetValue("startTime").ToString(), @"\w*(?=:)").ToString();
        string _startMinutes = Regex.Match(key.GetValue("startTime").ToString(), @"\d{2}(?=[\d\D]{0}$)").ToString();
        string _endHours = Regex.Match(key.GetValue("endTime").ToString(), @"\w*(?=:)").ToString();
        string _endMinutes = Regex.Match(key.GetValue("endTime").ToString(), @"\d{2}(?=[\d\D]{0}$)").ToString();

        int _startHours1 = int.Parse(_startHours);
        int _startMinutes1 = int.Parse(_startMinutes);
        int _endHours1 = int.Parse(_endHours);
        int _endMinutes1 = int.Parse(_endMinutes);

        startTimeHours.SelectedIndex = _startHours1;
        endTimeHours.SelectedIndex = _endHours1;
        startTimeMinutes.SelectedIndex = _startMinutes1;
        endTimeMinutes.SelectedIndex = _endMinutes1;
        key.Close();
    }

    private async void OpenDialog(string title, string connect)
    {
        var result = await _dialogControl.ShowAndWaitAsync(title, connect);
    }


    private void DialogControlOnButtonLeftClick(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        key.SetValue("SunRiseSet", "true");
        key.Close();
        setSunRise();
        dialogControl.Hide();
    }

    private void DialogControlOnButtonRightClick(object sender, RoutedEventArgs e)
    {
        var dialogControl = (IDialogControl)sender;
        dialogControl.Hide();
        SunRiseSet.IsChecked= false;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        //获取开始时间
        string startHours = Regex.Match(startTimeHours.SelectionBoxItem.ToString(),@"\d{2}(?=[\d\D]{1}$)").ToString();
        string startMinutes = Regex.Match(startTimeMinutes.SelectionBoxItem.ToString(),@"\d{2}(?=[\d\D]{1}$)").ToString();
        //获取结束时间
        string endHours = Regex.Match(endTimeHours.SelectionBoxItem.ToString(), @"\d{2}(?=[\d\D]{1}$)").ToString();
        string endMinutes = Regex.Match(endTimeMinutes.SelectionBoxItem.ToString(), @"\d{2}(?=[\d\D]{1}$)").ToString();

        //整合数据
        string startTime = startHours + ":" + startMinutes;
        string endTime = endHours + ":" + endMinutes;

        key.SetValue("startTime", startTime);
        key.SetValue("endTime", endTime);
        key.Close();
    }

    private async void setSunRise()
    {
        try
        {
            LocationService locationService = new LocationService();
            var geoLocator = new Geolocator();
            var geoPosition = await geoLocator.GetGeopositionAsync();
            var latitude = geoPosition.Coordinate.Point.Position.Latitude;
            var longitude = geoPosition.Coordinate.Point.Position.Longitude;
            var locationName = await locationService.GetLocationName(latitude, longitude);

            lat.Text = longitude.ToString();
            lng.Text = latitude.ToString();
            location.Text = locationName;

            SunTimeResult result = TimeConverter.GetSunTime(DateTime.Now, longitude, latitude);
            DateTime date = DateTime.Now;
            string sunriseTime = result.SunriseTime.ToString("HH:mm");
            string sunsetTime = result.SunsetTime.ToString("HH:mm");

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
            key.SetValue("startTime", sunriseTime);
            key.SetValue("endTime", sunsetTime);
            key.Close();

            UpdateTime();

            SunRiseSwitch.IsExpanded = true;
            startTimeHours.IsEnabled = false;
            endTimeHours.IsEnabled = false;
            startTimeMinutes.IsEnabled = false;
            endTimeMinutes.IsEnabled = false;
        }catch(Exception ex)
        {
            OpenSnackbar(LanguageHandler.GetLocalizedString("SetTimesPage_Tip3"), LanguageHandler.GetLocalizedString("SetTimesPage_Tip4"));
            log.Warn(ex);
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
            key.SetValue("SunRiseSet", "false");
            key.Close();
            location.Text = "";
            lat.Text = "";
            lng.Text = "";
            SunRiseSwitch.IsExpanded = false;
            startTimeHours.IsEnabled = true;
            endTimeHours.IsEnabled = true;
            startTimeMinutes.IsEnabled = true;
            endTimeMinutes.IsEnabled = true;
        }
        
    }

    private LightSensor _lightSensor;

    // 感光模式
    private void Autostart_Click(object sender, RoutedEventArgs e)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
        string state = key.GetValue("PhotosensitiveMode").ToString();
        if (state == "false")
        {
            _lightSensor = LightSensor.GetDefault();
            if (_lightSensor != null)
            {
                key.SetValue("PhotosensitiveMode", "true");
                Autostart.IsChecked = true;
            }
            else
            {
                key.SetValue("PhotosensitiveMode", "false");
                OpenSnackbar(LanguageHandler.GetLocalizedString("SetTimesPage_Tip5"), LanguageHandler.GetLocalizedString("SetTimesPage_Tip6"));
                Autostart.IsChecked = false;
            }
        }
        key.Close();
        
    }
    public static int GetBrightness()
    {
        IntPtr primaryMonitorHandle = MonitorFromWindow(IntPtr.Zero, MonitorBrightness);
        if (GetMonitorBrightness(primaryMonitorHandle, out uint minBrightness, out uint currentBrightness, out uint maxBrightness))
        {
            int brightnessPercentage = (int)(((double)currentBrightness - minBrightness) / (maxBrightness - minBrightness) * 100);
            return brightnessPercentage;
        }
        else
        {
            return -1;
        }
    }
}
