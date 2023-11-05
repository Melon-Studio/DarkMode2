using System.Collections.Concurrent;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DarkMode_2.Services.MessageQueue;

public class QueueService
{
    private static BlockingCollection<Action> messageQueue = new BlockingCollection<Action>();
    private static ManualResetEventSlim signal = new ManualResetEventSlim(false);
    public QueueService() 
    { 
        InitQueueService();
    }
    public void InitQueueService()
    {
        Task.Run(() => PreProcessMessage());
    }

    public void AddMessage(Action action)
    {
        messageQueue.Add(action);
    }

    void PreProcessMessage()
    {
        foreach (var action in messageQueue.GetConsumingEnumerable())
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                action();
            });

            if (messageQueue.Count == 0)
            {
                signal.Reset();
                signal.Wait();
            }
        }
    }
}
