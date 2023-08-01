using System;
using System.Diagnostics;

namespace DarkMode_2.Models
{
    public class WallpaperChanger
    {
        public static string GetWallpaperPath(string appPath)
        {
            string command = $"\"{appPath}\" -control getWallpaper";
            return ExecuteCommand(command);
        }

        public static void SetWallpaper(string appPath, string wallpaperPath)
        {
            string command = $"\"{appPath}\" -control openWallpaper -file \"{wallpaperPath}\"";
            ExecuteCommand(command);
        }

        private static string ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}
