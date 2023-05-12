using Microsoft.Win32.TaskScheduler;
using System.Linq;

namespace DarkMode_2.Models
{
    public static class StartupHelper
    {
        private const string TaskName = "DarkMode2";
        private const string TaskDescription = "DarkMode2自启动任务计划";
        private static readonly string TaskPath = $@"\{TaskName}";

        public static void Enable()
        {
            using (var taskService = new TaskService())
            {
                var taskDefinition = taskService.NewTask();
                taskDefinition.RegistrationInfo.Description = TaskDescription;

                var bootTrigger = new BootTrigger();
                taskDefinition.Triggers.Add(bootTrigger);

                var action = new ExecAction(System.Reflection.Assembly.GetEntryAssembly().Location);
                taskDefinition.Actions.Add(action);

                taskService.RootFolder.RegisterTaskDefinition(TaskPath, taskDefinition);
            }
        }

        public static void Disable()
        {
            using (var taskService = new TaskService())
            {
                taskService.RootFolder.DeleteTask(TaskPath, false);
            }
        }

        public static bool IsEnabled()
        {
            using (var taskService = new TaskService())
            {
                var task = taskService.RootFolder.GetTasks().FirstOrDefault(t => t.Path.EndsWith(TaskPath));
                return task != null;
            }
        }
    }
}


