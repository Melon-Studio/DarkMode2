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
        public bool UpdateVersionCompared(Version OldV, Version NewV);

        // Version类格式规范
        public Version VersionStandardFormat();

        // 更新相关JSON解析
        public string UpdateJsonInterpreter(string res, type type);

        // 获取最新版本Tag值
        public Task<string> UpdateVersionTagName();

        // 获取更新公告
        public Task<string> UpdateContent(string tagName);

        // 获取更新地址
        public Task<string> UpdateUrl(string tagName);

        // 获取更新日期
        public Task<string> UpdateDate(string tagName);
    }
}
