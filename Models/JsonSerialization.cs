using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace DarkMode_2.Models;

public class JsonSerialization
{
    public string ParseJson(string json) //返回版本名称
    {
        JObject keyValuePairs = JObject.Parse(json);
        string tag_name = Regex.Match(keyValuePairs["tag_name"]?.ToString(), @"(.*?)(?=-)").Groups[1].Value; 

        return tag_name;
    }
    public string ParseTagName(string json)
    {
        JObject keyValuePairs = JObject.Parse(json);
        string tag_name = keyValuePairs["tag_name"]?.ToString();

        return tag_name;
    }
    public string FileDownloadUrl(string json) //返回版本下载地址
    {
        JObject keyValuePairs = JObject.Parse(json);
        string url = keyValuePairs["assets"]["browser_download_url"].ToString();
        return url;
    }
    public string FileUpdateTime(string json) //返回版本创建时间
    {
        JObject keyValuePairs = JObject.Parse(json);
        string time;
        try
        {
            time = Regex.Replace(keyValuePairs["assets"]["created_at"].ToString(), @"(.+(?= T))", "");
        }
        finally
        {
            time = Regex.Replace(keyValuePairs["created_at"].ToString(), @"(.+(?= T))", "");
        }
        return time;
    }
}
