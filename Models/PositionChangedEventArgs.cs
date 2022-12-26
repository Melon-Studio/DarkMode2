using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models;

public class AddressResolverEventArgs : PositionChangedEventArgs
{
    /// <summary>
    /// 地址1
    /// </summary>
    public string Address1 { get; set; }
    /// <summary>
    /// 地址2
    /// </summary>
    public string Address2 { get; set; }
    /// <summary>
    /// 地址3
    /// </summary>
    public string Address3 { get; set; }
    public AddressResolverEventArgs()
    {

    }
}
public class PositionChangedEventArgs : EventArgs
{
    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; set; }

    public object Tag { get; set; }

    public PositionChangedEventArgs()
    {

    }
}
