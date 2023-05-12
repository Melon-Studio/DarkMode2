using System.Windows;

namespace DarkMode_2.Models;

public class MessageBox
{

    private static string Content;
    public static void OpenMessageBox(string title, string content)
    {
        Content = content;
        var messageBox = new Wpf.Ui.Controls.MessageBox
        {
            ButtonRightName = "关闭",

            ButtonLeftName = "确定"
        };

        messageBox.ButtonRightClick += MessageBox_RightButtonClick;

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;

        messageBox.ButtonLeftName = "复制";
        messageBox.ButtonRightName = "关闭";

        messageBox.Show(title, content);
    }

    private static void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
        Clipboard.SetText(Content);
    }
    private static void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }
}
