using System;

namespace DarkMode_2.Models
{
    public class WallpaperChanger
    {
        private string _wallpaperEnginePath;
        private string _wallpaperFilePath;

        public WallpaperChanger(string wallpapernEginePath, string wallpaperFilePath)
        {
            _wallpaperEnginePath = wallpapernEginePath;
            _wallpaperFilePath = wallpaperFilePath;
        }
        public void openWallpaper()
        {
            string command = $"{_wallpaperEnginePath} -control openWallpaper -file {_wallpaperFilePath}";
            Console.WriteLine("小子看这里："+command);
            CommandLine run = new CommandLine(command);
        }

        // Wallpaper Engine官方对于这个命令存在bug
        public string getNowWallpaper()
        {
            string command = $"{_wallpaperEnginePath} -control getWallpaper";
            CommandLine run = new CommandLine(command);
            return run.CommandOutput.Trim();
        }


    }
}
