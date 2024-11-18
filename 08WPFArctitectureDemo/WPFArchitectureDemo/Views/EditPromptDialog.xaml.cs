using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFArchitectureDemo.UI.ViewModels;

namespace WPFArchitectureDemo.UI.Views
{
    /// <summary>
    /// EditPromptDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditPromptDialog : Window
    {
        public EditPromptDialog(PromptViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
        }
        public PromptViewModel  ViewModel
        {
            get
            {
                return (PromptViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PromptViewModel), typeof(EditPromptDialog), new PropertyMetadata(null));
    }
}
