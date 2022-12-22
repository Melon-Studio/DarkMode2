namespace DarkMode_2.Models;

public class StartUrl
{
    public static void StartUrlLink(string url)
    {
        System.Diagnostics.Process.Start(url);
    }
}
