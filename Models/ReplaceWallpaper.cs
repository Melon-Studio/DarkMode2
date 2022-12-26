using System.IO;
using System.Runtime.InteropServices;

namespace DarkMode_2.Models;

public class ReplaceWallpaper
{
    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    public static bool ChangeNativeWallpaper(string FilePath)
    {
        bool result = false;
        if(File.Exists(FilePath))
        {
            string filePath = Path.GetFullPath(FilePath);
            SystemParametersInfo(20, 1, FilePath, 0x1 | 0x2);
            result = true;
        }
        
        return result;
    }
}
