using System;
using System.Device.Location;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace DarkMode_2.Models
{
    public class LocationService
    {
        private const string BingMapsKey = "9POfGrKXu2XVUBErnEDA~3azXh-0YKYqoT4IjnFnMog~Ajfj1WaDN-iosAXUuTjX02P8d5tXv8c6rfKg31cm50Qw6Ug8q5Ns2CVdGY-jebpE";

        private readonly HttpClient _httpClient;

        public LocationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetLocationName(double latitude, double longitude)
        {
            var coordinate = new GeoCoordinate(latitude, longitude);

            // Build the Bing Maps API request URL with the Chinese language and district-level location information
            var url = string.Format("https://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=json&key={2}&c=zh-Hans&inclnb=1&incl=ciso2&ul=39.9,-105.3&lvl=19",
                coordinate.Latitude, coordinate.Longitude, BingMapsKey);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
        {
            new JsonStringEnumConverter()
        }
            };
            var locationData = JsonSerializer.Deserialize<LocationData>(json, options);

            var address = locationData?.ResourceSets?[0]?.Resources?[0]?.Address;
            if (address == null)
            {
                return null;
            }

            // Use the CISO2 country code to format the address with the correct name for the country.
            // (e.g. "China" instead of "People's Republic of China")
            var countryCode = locationData.ResourceSets[0].Resources[0].Address.CountryRegionIso2;
            var countryName = new RegionInfo(countryCode)?.NativeName ?? address.CountryRegion;

            // Build the location name string with the desired order of address components.
            var locationName = $"{countryName}, {address.AdminDistrict}, {address.Locality}";

            // If the locality is blank, use the admin district 2 as the locality.
            if (string.IsNullOrEmpty(address.Locality))
            {
                locationName = $"{countryName}, {address.AdminDistrict}, {address.AdminDistrict2}";
            }

            return locationName;
        }
    }

    public class LocationData
    {
        public ResourceSet[] ResourceSets { get; set; }
    }

    public class ResourceSet
    {
        public Resource[] Resources { get; set; }
    }

    public class Resource
    {
        public Address Address { get; set; }
    }

    public class Address
    {
        public string FormattedAddress { get; set; }
        public string Locality { get; set; }
        public string AdminDistrict { get; set; }
        public string AdminDistrict2 { get; set; }
        public string CountryRegion { get; set; }
        public string CountryRegionIso2 { get; set; }
    }
}
