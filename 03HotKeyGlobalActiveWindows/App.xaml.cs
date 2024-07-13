using Hardcodet.Wpf.TaskbarNotification;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace HotKeyGlobalActiveWindows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TaskbarIcon TaskbarIcon { get; set; }
        public ICommand ShowMainWindowCommand { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShowMainWindowCommand = new ShowMainWindowCommand();
            TaskbarIcon = (TaskbarIcon)FindResource("Taskbar");
            TaskbarIcon.DataContext = this; // 设置 DataContext 以便绑定命令
        }
    }
    public class ShowMainWindowCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }
}
