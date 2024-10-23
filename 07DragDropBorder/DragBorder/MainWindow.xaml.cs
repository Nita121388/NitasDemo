using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragBorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
        public List<string> Data1 { get; set; } = new List<string>() { "Data from DragBorder1" };
        public List<string> Data2 { get; set; } = new List<string>() { "DragBorder2" };
    }
}