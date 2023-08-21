using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models.Interface
{
    public interface IUpdate
    {
        public static class Consts
        {
            internal const string githubUrl = "https://api.github.com/repos/Melon-Studio/DarkMode2/releases/latest";
            internal const string giteeUrl = "https://gitee.com/api/v5/repos/melon-studio/DarkMode2/releases/latest";
            internal const string username = "6get-xiaofan";
            internal const string token = "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"; //此 Token 仅限访问公开库的基本信息
            internal const string giteeToken = "e995460832ca545707451332283aadc1";  //此 Token 仅限访问公开库的基本信息
            internal const string githubIP = "192.30.255.113";
            internal const string giteeIP = "212.64.63.215";
        }

        public enum Channel
        {
            Github, 
            Gitee,
            TimeOut
        }

        public enum type
        {
            TagName,
            Content,
            DownloadUrl,
            Date
        }

        // 获取更新渠道
        public Channel UpdateChannel();

        // Ping IP 地址
        public Channel PingIp();

        // 版本对比: New V:TRUE | Old V:FALSE
        public string UpdateVersionCompared(Version OldV, Version NewV);

        // 更新相关JSON解析
        public Task<string> UpdateJsonInterpreter(string res, type type, Channel? channel);

        // 获取对应渠道JSON
        public Task<string> GetJson(Channel channel);
    }
}
