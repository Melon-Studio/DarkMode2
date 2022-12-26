using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace DarkMode_2.Models;

public delegate void OnPositionChangedEventHandle(object sender, PositionChangedEventArgs e);
public delegate void OnAddressResolveredEventHandle(object sender, AddressResolverEventArgs e);
