namespace DarkMode_2.Models;

public class VersionControl
{
    public static string Channel()
    {
        string channel = "Release";
        return channel;
    }
    public static string Version()
    {
        string version = "2.1.1";
        return version;
    }
    public static string InternalVersion()
    {
        string internalVersion = "20230809";
        return internalVersion;
    }
}
