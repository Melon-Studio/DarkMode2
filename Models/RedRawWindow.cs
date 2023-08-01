using System;
using System.Runtime.InteropServices;

namespace DarkMode_2.Models;

public class RedRawWindow
{
    [DllImport("user32.dll")]
    private static extern bool SendNotifyMessage(uint hWnd, uint Msg, IntPtr wPARAM, IntPtr lPARAM);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    public static bool ChangeColorMode()
    {
        const uint HWND_BROADCAST = 0xffff;
        const uint WM_SETTINGCHANGE = 0x001A;

        bool res = SendNotifyMessage(HWND_BROADCAST, WM_SETTINGCHANGE, new IntPtr(0), Marshal.StringToHGlobalAnsi("ImmersiveColorSet"));
        if (res)
        {
            return true;
        }
        return false;
    }

    public static bool RefreshSystemScheme()
    {
        const int SPI_SETCURSOR = 0x0057;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        bool res = SystemParametersInfo(SPI_SETCURSOR, 0, null, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        if (res)
        {
            return true;
        }
        return false;
    }

}