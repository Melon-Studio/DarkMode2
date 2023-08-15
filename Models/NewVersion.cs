using DarkMode_2.Models.Interface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models
{
    public class NewVersion : IUpdate
    {
        public IUpdate.Channel PingIp()
        {
            try
            {
                Ping ping1 = new Ping();
                PingReply reply1 = ping1.Send(IUpdate.Consts.githubIP, 1000);
                double latency1 = reply1.RoundtripTime;

                Ping ping2 = new Ping();
                PingReply reply2 = ping2.Send(IUpdate.Consts.giteeUrl, 1000);
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
                return PingIp();
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

        public Task<string> UpdateContent(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateDate(string tagName)
        {
            throw new NotImplementedException();
        }

        public string UpdateJsonInterpreter(string res, IUpdate.type type)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUrl(string tagName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateVersionCompared(Version OldV, Version NewV)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateVersionTagName()
        {
            throw new NotImplementedException();
        }

        public Version VersionStandardFormat()
        {
            throw new NotImplementedException();
        }
    }
}
