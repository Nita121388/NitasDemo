using Microsoft.Win32;
using Nita.ToolKit.NAudio.Entity;
using System.Windows;

namespace Nita.ToolKit.WaveDemo
{
    /// <summary>
    /// WaveUI.xaml 的交互逻辑
    /// </summary>
    public partial class WaveUI : System.Windows.Controls.UserControl
    {
        public WaveUI()
        {
            DataContext = this;
            InitializeComponent();
        }

        #region PeakCalculationStrategyProperty
        public PeakCalculationStrategy PeakCalculationStrategy
        {
            get { return (PeakCalculationStrategy)GetValue(PeakCalculationStrategyProperty); }
            set { SetValue(PeakCalculationStrategyProperty, value); }
        }

        public static readonly DependencyProperty PeakCalculationStrategyProperty =
            DependencyProperty.Register("PeakCalculationStrategy", typeof(PeakCalculationStrategy),
                typeof(WaveUI), new PropertyMetadata(PeakCalculationStrategy.Max_Absolute_Value));
        #endregion

        #region FilePath
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string),
                typeof(WaveUI), new PropertyMetadata());
        #endregion

        private void OnLoadSoundFileClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Files|*.mp3|WAV files|*.wav";
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
            }
        }
    }
}
