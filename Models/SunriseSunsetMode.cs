using DarkMode_2.Views.Pages;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DarkMode_2.Models.DarkModeException;

namespace DarkMode_2.Models;

public class SunriseSunsetMode
{
    private double _lat = 39.89491; //经度
    private double _lng = 116.322056; //纬度
    private string _Address; //地址
    private bool state = false;
    GeoCoordinateWatcher watcher = null;

    public double GetLat()
    {
        return _lat;
    }

    public double GetLng()
    {
        return _lng;
    }

    public string GetAddress()
    {
        SetAddress();
        Console.WriteLine(_Address);
        return _Address;
    }

    public void GetLocationEvent()
    {
        watcher = new();
        watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PostionChanged);
        state = watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
        if (state)
        {
            state = true;
        }
    }

    void watcher_PostionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
    {
        PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
    }

    async void PrintPosition(double Latitude, double Longitude)
    {
        await Task.Run(() =>
        {
            _lat = Latitude;
            _lng = Longitude;
        });
    }

    private void SetAddress()
    {
        string mapApiUrl = "http://dev.virtualearth.net/REST/v1/Locations/" + GetLat() + "," + GetLng() + "?includeEntityTypes=countryRegion,Address&o=json&key=9POfGrKXu2XVUBErnEDA~3azXh-0YKYqoT4IjnFnMog~Ajfj1WaDN-iosAXUuTjX02P8d5tXv8c6rfKg31cm50Qw6Ug8q5Ns2CVdGY-jebpE&c=zh-Hans";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mapApiUrl);
        request.Method = "GET";
        request.ContentType = "application/json;charset=UTF-8";
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();
        response.Close();
        JObject json = JObject.Parse(retString);
        this._Address = (string)json["resourceSets"][0]["resources"][0]["name"];
    }

}
