using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models;

public class DevicePositioning
{
    private CivicAddressResolver _address = null;
    private GeoCoordinateWatcher _location = null;
    private GeoCoordinate _lastPosition = GeoCoordinate.Unknown;
    private volatile bool _locationOn = true;
    private bool _resolverByPositionChanged = true;

    public event OnAddressResolveredEventHandle OnAddressResolvered;

    /// <summary>
    /// 当前位置
    /// </summary>
    public GeoCoordinate Position
    {
        get { return _lastPosition; }
    }


    public DevicePositioning()
    {
        _location = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        //
        _location.MovementThreshold = 1.0;//1米
        _location.PositionChanged += Location_PositionChanged;
        //
        _address = new CivicAddressResolver();
        _address.ResolveAddressCompleted += Address_ResolveAddressCompleted;
    }
    /// <summary>
    /// 异步定位取Position值
    /// </summary>
    public void Positioning()
    {
        bool started = false;
        _resolverByPositionChanged = _locationOn = true;
        try
        {
            started = _location.TryStart(true, TimeSpan.FromMilliseconds(1024));
            //_location.TryStart(
            //_location.Start(true);
            //
            if (started)
            {
                //if (_location.Position.Location.IsUnknown == false)
                //{
                //    _address.ResolveAddressAsync(_location.Position.Location);
                //}
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (!started && _locationOn)
            {
                System.Threading.Thread.Sleep(128);
                Positioning();
            }
        }
    }
    public void UnPositioning()
    {
        try
        {
            _locationOn = false;
            if (_location != null)
                _location.Stop();
        }
        catch (Exception ex)
        {
        }
    }

    public void AddressResolver(double lat, double lon)
    {
        InnerAddressResolver(new GeoCoordinate(lat, lon));
    }
    private void InnerAddressResolver(GeoCoordinate position)
    {
        try
        {
            _lastPosition = position;
            _address.ResolveAddressAsync(_lastPosition);
        }
        catch (Exception ex)
        {
        }
    }
    private void Location_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
    {
        try
        {
            _lastPosition = e.Position.Location;
            if (!_lastPosition.IsUnknown && _resolverByPositionChanged)
            {
                _address.ResolveAddressAsync(_lastPosition);
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            _resolverByPositionChanged = false;
        }
    }
    private void Address_ResolveAddressCompleted(object sender, ResolveAddressCompletedEventArgs e)
    {
        try
        {
            string address = Address(_lastPosition.Latitude, _lastPosition.Longitude);
            if (OnAddressResolvered != null)
            {
                OnAddressResolvered.BeginInvoke(this, new AddressResolverEventArgs()
                {
                    Longitude = _lastPosition.Longitude,
                    Latitude = _lastPosition.Latitude,
                    Address1 = e.Address.AddressLine1,
                    Address2 = e.Address.AddressLine2,
                    Address3 = address
                }, End_CallBack, null);
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    private void End_CallBack(IAsyncResult ar)
    {
        try
        {
            if (ar.IsCompleted)
            {
                if (OnAddressResolvered != null)
                    OnAddressResolvered.EndInvoke(ar);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private string Address(double lat, double lng)
    {
        string mapApiUrl = "http://dev.virtualearth.net/REST/v1/Locations/" + lat.ToString() + "," + lng.ToString() + "?includeEntityTypes=countryRegion,Address&o=json&key=9POfGrKXu2XVUBErnEDA~3azXh-0YKYqoT4IjnFnMog~Ajfj1WaDN-iosAXUuTjX02P8d5tXv8c6rfKg31cm50Qw6Ug8q5Ns2CVdGY-jebpE&c=zh-Hans";

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
        Console.WriteLine((string)json["resourceSets"][0]["resources"][0]["name"]);
        return (string)json["resourceSets"][0]["resources"][0]["name"];
    }
}
