using System;
using System.Management;
using System.Threading;
using System.Timers;
//using OpenHardwareMonitor.Hardware;

namespace DarkMode_2.Models
{
    class Program
    {
        static int threshold = 80; // 注册表中设置的阈值（占用率百分比）
        static System.Timers.Timer timer = new System.Timers.Timer(1000); // 每1秒执行一次
        //static void Main(string[] args)
        //{
        //    timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //    timer.Start();

        //    Console.WriteLine("Press any key to exit...");
        //    Console.ReadKey();
        //}

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (var item in searcher.Get())
            {
                int usage = Convert.ToInt32(item["AdapterRAM"]) * 100 / Convert.ToInt32(item["AdapterCompatibility"]);
                Console.WriteLine("GPU Usage: " + usage + "%");

                // 如果达到阈值，执行一些代码
                if (usage >= threshold)
                {
                    Console.WriteLine("GPU is heavily utilized. Taking action...");
                    // TODO: 执行你需要的代码
                }
            }
        }
    }
}
