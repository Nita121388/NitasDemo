using FastHotKeyForWPF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WPFArchitectureDemo.UI.ViewModels;

namespace WPFArchitectureDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PromptsViewModel ViewModel { get; }
        public MainWindow()
        {
            InitializeComponent(); 
            ViewModel = App.Current.Services.GetRequiredService<PromptsViewModel>();
            ViewModel.Refresh();
            DataContext = ViewModel;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GlobalHotKey.Awake();
        }

        private void OpenWindow(object sender, HotKeyEventArgs e)
        {
            this.Topmost = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            GlobalHotKey.Destroy();

            base.OnClosed(e);
        }
    }
}