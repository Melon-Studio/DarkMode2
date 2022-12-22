using System;
using System.Runtime.InteropServices;

namespace DarkMode_2.Models;

public class RedRawWindow
{
    [DllImport("user32.dll")]
    private static extern bool SendNotifyMessage(uint hWnd, uint Msg, IntPtr wPARAM, IntPtr lPARAM);

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

}