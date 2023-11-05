using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Shell;

namespace DarkMode_2.Models
{
    public class DownloadManager
    {
        private const string DownloadFolderName = "DarkModeDownloads";

        public event Action<double> ProgressChanged;
        public event Action<double> SpeedChanged;
        public event Action<string> DownloadCompleted;

        private HttpClient httpClient;
        private Dispatcher dispatcher;
        private DateTime startTime;
        private long totalBytesDownloaded;
        private double totalFileSize;

        public DownloadManager()
        {
            httpClient = new HttpClient();
            dispatcher = Application.Current.Dispatcher;
        }

        public async Task DownloadFileAsync(string url)
        {
            try
            {
                string downloadFolderPath = Path.Combine(KnownFolders.Downloads.Path, DownloadFolderName);
                if (!Directory.Exists(downloadFolderPath))
                {
                    Directory.CreateDirectory(downloadFolderPath);
                }

                string fileName = Path.GetFileName(url);
                string filePath = Path.Combine(downloadFolderPath, fileName);

                using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true))
                {
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int bytesRead;
                    totalFileSize = response.Content.Headers.ContentLength ?? -1;

                    startTime = DateTime.Now;
                    totalBytesDownloaded = 0;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalRead += bytesRead;
                        totalBytesDownloaded += bytesRead;

                        if (totalFileSize > 0)
                        {
                            double progress = (double)totalRead / totalFileSize;
                            ProgressChanged?.Invoke(progress);

                            double elapsedTimeInSeconds = (DateTime.Now - startTime).TotalSeconds;
                            double downloadSpeedMBps = totalBytesDownloaded / (elapsedTimeInSeconds * 1024 * 1024);
                            SpeedChanged?.Invoke(downloadSpeedMBps);
                        }
                    }
                }

                DownloadCompleted?.Invoke(filePath);
            }
            catch { }
        }

        public static string GetDownloadPath()
        {
            string downloadFolderPath = Path.Combine(KnownFolders.Downloads.Path, DownloadFolderName);
            return downloadFolderPath;
        }
    }
}
