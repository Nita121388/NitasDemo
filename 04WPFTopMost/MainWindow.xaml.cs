using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTopMost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.LostKeyboardFocus += Window_PreviewLostKeyboardFocus; 
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;//防止最大化时系统任务栏被遮盖
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TopMostButton_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (bool)this.TopMostButton.IsChecked;
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.TopMostButton.IsChecked == false) return;
            this.Topmost = true;
        }

        /// <summary>
        /// 窗口移动
        /// </summary>
        private void RooGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            var window = Window.GetWindow(thumb);

            if (window != null)
            {
                double minWidth = 200;
                double minHeight = 150;

                if (thumb.Name == "ResizeThumbRight" || thumb.Name == "ResizeThumbBottomRight")
                {
                    window.Width = Math.Max(window.Width + e.HorizontalChange, minWidth);
                }
                if (thumb.Name == "ResizeThumbBottom" || thumb.Name == "ResizeThumbBottomRight")
                {
                    window.Height = Math.Max(window.Height + e.VerticalChange, minHeight);
                }
                if (thumb.Name == "ResizeThumbLeft" || thumb.Name == "ResizeThumbTopLeft")
                {
                    window.Width = Math.Max(window.Width - e.HorizontalChange, minWidth);
                    window.Left += e.HorizontalChange;
                }
                if (thumb.Name == "ResizeThumbTop" || thumb.Name == "ResizeThumbTopLeft")
                {
                    window.Height = Math.Max(window.Height - e.VerticalChange, minHeight);
                    window.Top += e.VerticalChange;
                }
            }
        }


    }
}