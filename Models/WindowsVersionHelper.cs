using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models
{
    public class WindowsVersionHelper
    {
        public static string GetWindowsEdition()
        {
            string edition = "Unknown";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                if (os["Caption"] != null)
                {
                    string caption = os["Caption"].ToString();
                    if (caption.Contains("Windows 10"))
                    {
                        edition = "Windows 10";
                    }
                    else if (caption.Contains("Windows 8"))
                    {
                        edition = "Windows 8";
                    }
                    else if (caption.Contains("Windows 7"))
                    {
                        edition = "Windows 7";
                    }
                    else if (caption.Contains("Windows Vista"))
                    {
                        edition = "Windows Vista";
                    }
                    else if (caption.Contains("Windows XP"))
                    {
                        edition = "Windows XP";
                    }
                    else if (caption.Contains("Windows 11"))
                    {
                        edition = "Windows 11";
                    }
                }
            }
            return edition;
        }

        public static Version GetWindowsVersion()
        {
            return Environment.OSVersion.Version;
        }
    }
}
