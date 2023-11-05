using DarkMode_2.Models.Interface;
using log4net;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarkMode_2.Models
{
    public class NewVersion : IUpdate
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewVersion));

        private readonly HttpClient _httpClient = new HttpClient();

        private static IUpdate.Channel _nowChannel;
        public IUpdate.Channel PingIp()
        {
            try
            {
                Ping ping1 = new Ping();
                PingReply reply1 = ping1.Send(IUpdate.Consts.githubIP, 2000);
                double latency1 = reply1.RoundtripTime;

                Ping ping2 = new Ping();
                PingReply reply2 = ping2.Send(IUpdate.Consts.giteeIP, 2000);
                double latency2 = reply2.RoundtripTime;

                if (latency1 < latency2)
                {
                    return IUpdate.Channel.Github;
                }
                else if (latency2 < latency1)
                {
                    return IUpdate.Channel.Gitee;
                }
                else
                {
                    return IUpdate.Channel.Github;
                }
            }
            catch { }
            return IUpdate.Channel.TimeOut;
        }

        public IUpdate.Channel UpdateChannel()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
            string channel = key.GetValue("UpdateChannels").ToString();

            if (channel == "Auto")
            {
                _nowChannel = PingIp();
                return _nowChannel;
            }
            if (channel == "Github")
            {
                return IUpdate.Channel.Github;
            }
            if (channel == "Gitee")
            {
                return IUpdate.Channel.Gitee;
            }

            return IUpdate.Channel.Github;
        }

        public async Task<string> UpdateJsonInterpreter(string res, IUpdate.type type, IUpdate.Channel? channel)
        {
            string result = null;
            JObject keyValuePairs;
            switch (type)
            {
                case IUpdate.type.TagName://新版本TagName

                    keyValuePairs = JObject.Parse(res);
                    string tag_name = Regex.Match(keyValuePairs["tag_name"]?.ToString(), @"(.*?)(?=-)").Groups[1].Value;
                    result = tag_name;
                    break;

                case IUpdate.type.Content://新版本内容

                    string apiUrl = null;
                    switch (channel)
                    {
                        case IUpdate.Channel.Github:
                            apiUrl = IUpdate.Consts.githubUrl;
                            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Code Sample Web Client");
                            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {IUpdate.Consts.token}");
                            break;
                        case IUpdate.Channel.Gitee:
                            apiUrl = Path.Combine(IUpdate.Consts.giteeUrl, $"?access_token={IUpdate.Consts.giteeToken}");
                            break;
                    }
                    
                    try
                    {
                        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();
                        JObject releaseInfo = JObject.Parse(responseContent);
                        string releaseBody = releaseInfo["body"]?.ToString();

                        if (!string.IsNullOrEmpty(releaseBody))
                        {
                            result = releaseBody;
                        }
                        else
                        {
                            result = LanguageHandler.GetLocalizedString("Update_Tip4");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        log.Error(ex);
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case IUpdate.type.DownloadUrl: //下载地址

                    keyValuePairs = JObject.Parse(res);
                    Console.WriteLine(keyValuePairs);
                    result = keyValuePairs["assets"][0]["browser_download_url"].ToString();
                    break;

                case IUpdate.type.Date: //发布日期

                    keyValuePairs = JObject.Parse(res);
                    result = Regex.Replace(keyValuePairs["created_at"].ToString(), @"(.+(?= T))", "");
                    break;
            }
            return result;
        }

        public string UpdateVersionCompared(Version OldV, Version NewV)
        {
            int comparisonResult = OldV.CompareTo(NewV);
            string need;
            if (comparisonResult < 0)
            {
                // 需要更新
                need = NewV.ToString();
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

        public async Task<string> GetJson(IUpdate.Channel channel)
        {
            switch (channel)
            {
                case IUpdate.Channel.Github:
                    try
                    {

                        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Code Sample Web Client");
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {IUpdate.Consts.token}");

                        HttpResponseMessage response = await _httpClient.GetAsync(IUpdate.Consts.githubUrl);
                        response.EnsureSuccessStatusCode();

                        string responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            return responseContent;
                        }
                    }
                    catch
                    {
                        return "error";
                    }
                    break;

                case IUpdate.Channel.Gitee:
                    string apiUrl = null;
                    apiUrl = Path.Combine(IUpdate.Consts.giteeUrl, $"?access_token={IUpdate.Consts.giteeToken}");
                    try
                    {
                        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();
                        string responseContent = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            return responseContent;
                        }
                    }
                    catch
                    {
                        return "error";
                    }
                    break;

                case IUpdate.Channel.TimeOut:
                    return "error";
            }
            return "error";
        }

        public async Task<string> CheckUpdate()
        {
            string res = await this.GetJson(this.UpdateChannel());
            if(res == "error")
            {
                return LanguageHandler.GetLocalizedString("Update_Tip3");
            }
            Version newVersion =new Version(await this.UpdateJsonInterpreter(res, IUpdate.type.TagName, _nowChannel));
            Version oldVersion = new Version(VersionControl.Version()+"."+VersionControl.InternalVersion());
            return this.UpdateVersionCompared(oldVersion, newVersion);
        }
    }
}
