namespace DarkMode_2.Models;

public class MessageBox
{
    public static void OpenMessageBox(string title, string content)
    {
        var messageBox = new Wpf.Ui.Controls.MessageBox
        {
            ButtonRightName = "关闭弹窗",

            ButtonLeftName = "确定"
        };

        messageBox.ButtonRightClick += MessageBox_RightButtonClick;

        messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;

        messageBox.Show(title, content);
    }

    private static void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }
    private static void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }
}
