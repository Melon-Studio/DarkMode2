using log4net;
using Microsoft.Win32;
using System;

namespace DarkMode_2.Models
{
    public static class RegistryInit
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegistryInit));
        public static void RegistryInitialization()
        {
            try
            {
                RegistryKey pan;
                RegistryKey key;
                pan = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2");

                if (pan == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(@"Software\DarkMode2");
                    key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
                    //软件安装路径
                    key.SetValue("DarkModeInstallPath", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                    //开机自启
                    key.SetValue("DarkMode2", "false");
                    //系统颜色初始化
                    key.SetValue("IsLight", "false");
                    //浅色模式开始时间
                    key.SetValue("startTime", "08:00");
                    //浅色模式结束时间
                    key.SetValue("endTime", "19:00");
                    //软件语言
                    key.SetValue("Language", "zh-CN");
                    //日出日落模式
                    key.SetValue("SunRiseSet", "false");
                    //感光模式
                    key.SetValue("PhotosensitiveMode", "false");
                    //自动更新日出日落时间
                    key.SetValue("AutoUpdateTime", "false");
                    //消息通知
                    key.SetValue("Notification", "true");
                    //托盘栏图标
                    key.SetValue("TrayBar", "true");
                    //软件主题色
                    key.SetValue("ColorMode", "Auto");
                    //软件自动更新
                    key.SetValue("AutoUpdate", "false");
                    //原生壁纸
                    key.SetValue("NativeLight", "");
                    key.SetValue("NativeDark", "");
                    //Wallpaper Engine壁纸
                    key.SetValue("WeLight", "");
                    key.SetValue("WeDark", "");
                    //更新渠道
                    key.SetValue("UpdateChannels", "Auto");
                    //鼠标主题
                    key.SetValue("SwitchMouse", "false");
                    key.SetValue("MouseMode", "Light");
                    key.SetValue("LightMouse", "Light");
                    key.SetValue("DarkMouse", "Light");
                    //触摸键盘主题
                    key.SetValue("KeyboardMode", "false");
                    //游戏模式
                    key.SetValue("GameMode", "false");
                    //Wallpaper Engine安装路径
                    key.SetValue("WeInstallPath", "");
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("注册表初始化失败："+ex);
            }
        }

        public static void RegistryReset()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
                //软件安装路径
                key.SetValue("DarkModeInstallPath", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                //开机自启
                key.SetValue("DarkMode2", "false");
                //系统颜色初始化
                key.SetValue("IsLight", "false");
                //浅色模式开始时间
                key.SetValue("startTime", "08:00");
                //浅色模式结束时间
                key.SetValue("endTime", "19:00");
                //软件语言
                key.SetValue("Language", "zh-CN");
                //日出日落模式
                key.SetValue("SunRiseSet", "false");
                //感光模式
                key.SetValue("PhotosensitiveMode", "false");
                //自动更新日出日落时间
                key.SetValue("AutoUpdateTime", "false");
                //消息通知
                key.SetValue("Notification", "true");
                //托盘栏图标
                key.SetValue("TrayBar", "true");
                //软件主题色
                key.SetValue("ColorMode", "Auto");
                //软件自动更新
                key.SetValue("AutoUpdate", "false");
                //原生壁纸
                key.SetValue("NativeLight", "");
                key.SetValue("NativeDark", "");
                //Wallpaper Engine壁纸
                key.SetValue("WeLight", "");
                key.SetValue("WeDark", "");
                //更新渠道
                key.SetValue("UpdateChannels", "Auto");
                //鼠标主题
                key.SetValue("SwitchMouse", "false");
                key.SetValue("MouseMode", "Light");
                key.SetValue("LightMouse", "Light");
                key.SetValue("DarkMouse", "Light");
                //触摸键盘主题
                key.SetValue("KeyboardMode", "false");
                //游戏模式
                key.SetValue("GameMode", "false");
                //Wallpaper Engine安装路径
                key.SetValue("WeInstallPath", "");
                key.Close();

            }
            catch (Exception ex)
            {
                log.Error("注册表重置失败：" + ex);
            }
        }

        public static void InsertRegistery(string key, string value)
        {
            try
            {
                RegistryKey appKey = Registry.CurrentUser.OpenSubKey(@"Software\DarkMode2", true);
                appKey.SetValue(key, value);
            }
            catch(Exception ex)
            {
                log.Error("注册表操作失败(InsertRegistery)：" + ex);
            }
            

        }
    }
}
