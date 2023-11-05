using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMode_2.Models;

public class ToastHelper
{
    public static void ShowToast(string title, string message)
    {
        try
        {
            new ToastContentBuilder()
                .AddArgument("conversationId", 98135474)
                .AddText(LanguageHandler.GetLocalizedString(title))
                .AddText(LanguageHandler.GetLocalizedString(message))
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddDays(1);
                });
        }catch (Exception ex)
        {
            MessageBox.OpenMessageBox(LanguageHandler.GetLocalizedString("ToastHelper_Error_title"), ex.ToString());
        }
    }
}
