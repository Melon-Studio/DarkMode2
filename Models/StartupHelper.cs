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
                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest; // 使用最高权限运行
                taskDefinition.Settings.Enabled = true; // 启用任务
                taskDefinition.Settings.StartWhenAvailable = true; // 在任何用户登录后都运行
                taskDefinition.Settings.DisallowStartIfOnBatteries = false; // 不管计算机是否使用交流电都要运
                taskDefinition.Triggers.Add(new BootTrigger()); // 不管用户是否登录都要运行
                taskDefinition.Triggers.Add(new IdleTrigger()); // 不管计算机是否空闲都要运行

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


