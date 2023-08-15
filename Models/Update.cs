using log4net;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DarkMode_2.Models;

public class Update
{
    public static int state;
    public static int useChannel;
    public static string need;
    public static string githubUrl = "https://api.github.com/repos/Melon-Studio/DarkMode2/releases/latest";
    public static string giteeUrl = "https://gitee.com/api/v5/repos/melon-studio/DarkMode2/releases/latest";
    private readonly HttpClient _httpClient = new HttpClient();
    private static readonly ILog log = LogManager.GetLogger(typeof(Update));

    private static string username = "6get-xiaofan";
    private static string token = "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"; //此 Token 仅限访问公开库的基本信息
    private static string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{token}"));
    public int UpdateChannels()
    {
        RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        string channel = appkey.GetValue("UpdateChannels").ToString();
        if(channel == "Auto")
        {
            long x = PingIP("github");
            long y = PingIP("gitee");

            if(x > y)
            {
                useChannel = 1;
            }else if(y > x)
            {
                useChannel = 0;
            }else if(x == y)
            {
                useChannel = -1;
            }
        }else if(channel == "Github")
        {
            useChannel = 0;
        }else if(channel == "Gitee")
        {
            useChannel = 1;
        }
        return useChannel;
    }
    public async Task<string> CheckUpdate()
    {
        int usestate = UpdateChannels();
        string version =VersionControl.Version() + "." + VersionControl.InternalVersion();

        if(usestate == 0) //github
        {
            string result = await Get(githubUrl, "github");
            JsonSerialization jsonSerialization = new JsonSerialization();

            Version oldVersion = new Version(version);
            Version newVersion = new Version(jsonSerialization.ParseJson(result).ToString());
            string tagName = jsonSerialization.ParseTagName(result);

            int comparisonResult = oldVersion.CompareTo(newVersion);

            need = CheckVersion(comparisonResult, tagName);
        }
        else if(usestate == 1) //gitee
        {
            string result = await Get(giteeUrl, "gitee");
            JsonSerialization jsonSerialization = new JsonSerialization();

            Version oldVersion = new Version(version);
            Version newVersion = new Version(jsonSerialization.ParseJson(result).ToString());
            string tagName = jsonSerialization.ParseTagName(result);

            int comparisonResult = oldVersion.CompareTo(newVersion);

            need = CheckVersion(comparisonResult, tagName);
        }
        else if(usestate == -1)//连接超时
        {
            need = LanguageHandler.GetLocalizedString("Update_Tip3");
        }
        return need;
    }

    private static string CheckVersion(int comparisonResult, string newVersion)
    {
        string need;
        if (comparisonResult < 0)
        {
            // 需要更新
            need = newVersion.ToString();
        }
        else if (comparisonResult == 0)
        {
            // 最新版本
            need = LanguageHandler.GetLocalizedString("Update_Tip1");
        }
        else
        {
            // 旧版本比新版本新
            need = "不要乱动注册表！难道你在开发吗? :D :D :D";
        }
        return need;
    }
    public long PingIP(string IP)
    {
        var githubIP = "192.30.255.113"; //GitHub
        var giteeIP = "212.64.63.215"; //Gitee
        long bRet = 99999;

        if(IP == "github")
        {
            try
            {
                Ping pingSend = new Ping();
                PingReply reply = pingSend.Send(githubIP, 1000);
                bRet = reply.RoundtripTime;
            }
            catch (Exception)
            {
                bRet = 99999;
            }
        }else if(IP == "gitee")
        {
            try
            {
                Ping pingSend = new Ping();
                PingReply reply = pingSend.Send(giteeIP, 1000);
                bRet = reply.RoundtripTime;
            }
            catch (Exception)
            {
                bRet = 99999;
            }
        }
        return bRet;
    }

    public async Task<string> Get(string url, string channel)
    {
        string result = "";
        if(channel == "github")
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Code Sample Web Client");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                result = responseContent;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }
        else
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                stream.Close();
            }
        }
        
        return result;
    }
    public async Task<string> UpdateContent(string tagName)
    {
        string apiUrl = $"https://api.github.com/repos/Melon-Studio/DarkMode2/releases/tags/{tagName}";
        try
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Code Sample Web Client");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            JObject releaseInfo = JObject.Parse(responseContent);
            string releaseBody = releaseInfo["body"]?.ToString();

            if (!string.IsNullOrEmpty(releaseBody))
            {
                return releaseBody;
            }
        }
        catch(HttpRequestException ex)
        {
            log.Error(ex);
            Console.WriteLine(ex.Message);
        }
        return LanguageHandler.GetLocalizedString("Update_Tip4");
    }

}