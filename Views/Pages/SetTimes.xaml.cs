using DarkMode_2.Models;
using DarkMode_2.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using static log4net.Appender.RollingFileAppender;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// SetTimes.xaml 的交互逻辑
/// </summary>
public partial class SetTimes
{

    private readonly ISnackbarService _snackbarService;

    private readonly IDialogControl _dialogControl;

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
        key.Close();
    }

    private void BingData()
    {
        Dictionary<int,string> keyValuePairs = new Dictionary<int,string>();
        keyValuePairs.Add(1, "00");
        keyValuePairs.Add(2, "01");
        keyValuePairs.Add(3, "02");
        keyValuePairs.Add(4, "03");
        keyValuePairs.Add(5, "04");
        keyValuePairs.Add(6, "05");
        keyValuePairs.Add(7, "06");
        keyValuePairs.Add(8, "07");
        keyValuePairs.Add(9, "08");
        keyValuePairs.Add(10, "09");
        keyValuePairs.Add(11, "10");
        keyValuePairs.Add(12, "11");
        keyValuePairs.Add(13, "12");
        keyValuePairs.Add(14, "13");
        keyValuePairs.Add(15, "14");
        keyValuePairs.Add(16, "15");
        keyValuePairs.Add(17, "16");
        keyValuePairs.Add(18, "17");
        keyValuePairs.Add(19, "18");
        keyValuePairs.Add(20, "19");
        keyValuePairs.Add(21, "20");
        keyValuePairs.Add(22, "21");
        keyValuePairs.Add(23, "22");
        keyValuePairs.Add(24, "23");
        startTimeHours.ItemsSource = keyValuePairs;
        endTimeHours.ItemsSource = keyValuePairs;
        startTimeHours.SelectedIndex = 0;
        endTimeHours.SelectedIndex = 0;

        Dictionary<int, string> keyValuePairs1 = new Dictionary<int, string>();
        keyValuePairs1.Add(0, "00");
        keyValuePairs1.Add(2, "01");
        keyValuePairs1.Add(3, "02");
        keyValuePairs1.Add(4, "03");
        keyValuePairs1.Add(5, "04");
        keyValuePairs1.Add(6, "05");
        keyValuePairs1.Add(7, "06");
        keyValuePairs1.Add(8, "07");
        keyValuePairs1.Add(9, "08");
        keyValuePairs1.Add(10, "09");
        keyValuePairs1.Add(11, "10");
        keyValuePairs1.Add(12, "11");
        keyValuePairs1.Add(13, "12");
        keyValuePairs1.Add(14, "13");
        keyValuePairs1.Add(15, "14");
        keyValuePairs1.Add(16, "15");
        keyValuePairs1.Add(17, "16");
        keyValuePairs1.Add(18, "17");
        keyValuePairs1.Add(19, "18");
        keyValuePairs1.Add(20, "19");
        keyValuePairs1.Add(21, "20");
        keyValuePairs1.Add(22, "21");
        keyValuePairs1.Add(23, "22");
        keyValuePairs1.Add(24, "23");
        keyValuePairs1.Add(25, "24");
        keyValuePairs1.Add(26, "25");
        keyValuePairs1.Add(27, "26");
        keyValuePairs1.Add(28, "27");
        keyValuePairs1.Add(29, "28");
        keyValuePairs1.Add(30, "29");
        keyValuePairs1.Add(31, "30");
        keyValuePairs1.Add(32, "31");
        keyValuePairs1.Add(33, "32");
        keyValuePairs1.Add(34, "33");
        keyValuePairs1.Add(35, "34");
        keyValuePairs1.Add(36, "35");
        keyValuePairs1.Add(37, "36");
        keyValuePairs1.Add(38, "37");
        keyValuePairs1.Add(39, "38");
        keyValuePairs1.Add(40, "39");
        keyValuePairs1.Add(41, "40");
        keyValuePairs1.Add(42, "41");
        keyValuePairs1.Add(43, "42");
        keyValuePairs1.Add(44, "43");
        keyValuePairs1.Add(45, "44");
        keyValuePairs1.Add(46, "45");
        keyValuePairs1.Add(47, "46");
        keyValuePairs1.Add(48, "47");
        keyValuePairs1.Add(49, "48");
        keyValuePairs1.Add(50, "49");
        keyValuePairs1.Add(51, "50");
        keyValuePairs1.Add(52, "51");
        keyValuePairs1.Add(53, "52");
        keyValuePairs1.Add(54, "53");
        keyValuePairs1.Add(55, "54");
        keyValuePairs1.Add(56, "55");
        keyValuePairs1.Add(57, "56");
        keyValuePairs1.Add(58, "57");
        keyValuePairs1.Add(59, "58");
        keyValuePairs1.Add(60, "59");
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
            OpenDialog("是否允许 DarkMode 访问你的精确位置", "\nDarkMode 使用 Windows 位置服务获取设备所在地的地理位置，并计算出日出日落时间。");
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
        _snackbarService.Show(title, connect, SymbolRegular.FoodCake24);
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
    }

}
