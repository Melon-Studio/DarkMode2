using Microsoft.Win32;

namespace DarkMode_2.Models
{
    public class AutoStartManager
    {
        private const string RunRegistryPath = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string RunRegistryKeyName = "DarkMode 2";

        public static bool IsAutoStartEnabled()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(RunRegistryPath, false))
            {
                return (key.GetValue(RunRegistryKeyName) != null);
            }
        }

        public static void EnableAutoStart()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(RunRegistryPath, true))
            {
                key.SetValue(RunRegistryKeyName, "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\"");
            }
        }

        public static void DisableAutoStart()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(RunRegistryPath, true))
            {
                key.DeleteValue(RunRegistryKeyName, false);
            }
        }
    }
}
