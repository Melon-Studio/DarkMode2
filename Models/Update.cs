using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;

namespace DarkMode_2.Models;

public class Update
{
    public static int state;
    public static int useChannel;
    public static string need;
    public int UpdateChannels()
    {
        RegistryKey appkey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", false);
        string channel = appkey.GetValue("UpdateChannels").ToString();
        if(channel == "Auto")
        {
            long x = PingIP(0);
            long y = PingIP(1);

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
    public string CheckUpdate()
    {
        int usestate = UpdateChannels();
        string version =VersionControl.Version();

        if(usestate == 0) //github
        {
            string result = Get("https://api.github.com/repos/Melon-Studio/DarkMode/releases/latest");
            JsonSerialization jsonSerialization = new JsonSerialization();

            Version oldVersion = new Version(version);
            Version newVersion = new Version(jsonSerialization.ParseJson(result).ToString());

            if (oldVersion.CompareTo(newVersion) > 0)
            {
                //最新版本
                need = LanguageHandler.GetLocalizedString("Update_Tip1");
            }
            else
            {
                //需要更新
                need = LanguageHandler.GetLocalizedString("Update_Tip2") + jsonSerialization.ParseJson(result).ToString();
            }
        }else if(usestate == 1)//gitee
        {
            string result = Get("https://gitee.com/api/v5/repos/melon_studio/darkmode/releases/latest");
            JsonSerialization jsonSerialization = new JsonSerialization();

            Version oldVersion = new Version(version);
            Version newVersion = new Version(jsonSerialization.ParseJson(result).ToString());

            if (oldVersion.CompareTo(newVersion) > 0)
            {
                //最新版本
                need = LanguageHandler.GetLocalizedString("Update_Tip1");
            }
            else
            {
                //需要更新
                need = LanguageHandler.GetLocalizedString("Update_Tip2") + jsonSerialization.ParseJson(result).ToString();
            }
        }else if(usestate == -1)//连接超时
        {
            need = LanguageHandler.GetLocalizedString("Update_Tip3");
        }
        return need;
    }
    public long PingIP(int IP)
    {
        var githubIP = "192.30.255.113"; //GitHub
        var giteeIP = "212.64.63.215"; //Gitee
        long bRet = 99999;

        if(IP == 0)
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
        }else if(IP == 1)
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

    public static string Get(string url)
    {
        string result = "";
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Code Sample Web Client";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取内容
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        catch
        {

        }
        finally
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Code Sample Web Client";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            stream.Close();
        }
        return result;
    }
}
