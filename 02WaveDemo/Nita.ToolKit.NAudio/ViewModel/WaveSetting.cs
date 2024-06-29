using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using Nita.ToolKit.NAudio.Entity;
using Nita.ToolKit.NAudio.Controls.View;
using System.Collections.ObjectModel;
using NAudio.Wave;
using System.Windows.Forms;

namespace Nita.ToolKit.NAudio.ViewModel
{
    public class WaveSetting : ObservableObject
    {

        #region FilePath
        private string _filepath;
        public string FilePath
        {
            get { return _filepath; }
            set
            {
                SetProperty(ref _filepath, value);
                Refresh();
            }
        }
        #endregion

        #region WaveBrush

        private Brush _waveBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#70db74"));
        public Brush WaveBrush
        {
            get { return _waveBrush; }
            set
            {
                SetProperty(ref _waveBrush, value);
            }
        }
        #endregion

        #region SpacerColor
        private Brush _spacerColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        public Brush SpacerColor
        {
            get { return _spacerColor; }
            set
            {
                SetProperty(ref _spacerColor, value);
            }
        }
        #endregion

        #region PixelsPerPeak

        private int _pixelsPerPeak = 3;
        public int PixelsPerPeak
        {
            get { return _pixelsPerPeak; }
            set { SetProperty(ref _pixelsPerPeak, value); }
        }
        #endregion

        #region BlockSize
        private int _blockSize = 100;
        public int BlockSize
        {
            get { return _blockSize; }
            set { SetProperty(ref _blockSize, value); }
        }
        #endregion

        #region SpacerPixels 
        private int _spacerPixels = 1;
        public int SpacerPixels
        {
            get { return _spacerPixels; }
            set { SetProperty(ref _spacerPixels, value); }
        }
        #endregion

        #region TopHeight
        private int _topHeight = 50;
        public int TopHeight
        {
            get { return _topHeight; }
            set { SetProperty(ref _topHeight, value); }
        }
        #endregion

        #region BottomHeight    
        private int _bottomHeight = 100;
        public int BottomHeight
        {
            get { return _bottomHeight; }
            set { SetProperty(ref _bottomHeight, value); }
        }
        #endregion

        #region PeakCalculationStrategy

        public PeakCalculationStrategy _peakCalculationStrategy = PeakCalculationStrategy.Max_Absolute_Value;
        public PeakCalculationStrategy PeakCalculationStrategy
        {
            get { return _peakCalculationStrategy; }
            set
            {
                SetProperty(ref _peakCalculationStrategy, value);
                Refresh();
            }
        }
        #endregion

        #region PeakProvider

        public IPeakProvider _peakProvider;
        public IPeakProvider PeakProvider
        {
            get
            {
                if (PeakCalculationStrategy == PeakCalculationStrategy.Max_Absolute_Value)
                {
                    _peakProvider = new MaxPeakProvider();
                }
                else if (PeakCalculationStrategy == PeakCalculationStrategy.Max_Rms_Value)
                {
                    _peakProvider = new RmsPeakProvider(BlockSize);
                }
                else if (PeakCalculationStrategy == PeakCalculationStrategy.Sampled_Peaks)
                {
                    _peakProvider = new SamplingPeakProvider(BlockSize);
                }
                else if (PeakCalculationStrategy == PeakCalculationStrategy.Average)
                {
                    _peakProvider = new AveragePeakProvider(4);
                }
                return _peakProvider;
            }
        }
        #endregion

        #region ItemsSource

        public ObservableCollection<WaveformItem> _itemsSource;
        public ObservableCollection<WaveformItem> ItemsSource
        {
            get { return _itemsSource; }
            set { SetProperty(ref _itemsSource, value); }
        }
        #endregion

        public void Refresh()
        {
            try
            {
                if (string.IsNullOrEmpty(FilePath)) return;
                var rendererSettings = new WaveFormRendererSettings()
                {
                    PixelsPerPeak = PixelsPerPeak,
                    SpacerPixels = SpacerPixels,
                    WaveBrush = WaveBrush,
                    SpacerBrush = SpacerColor,
                    TopHeight = TopHeight,
                    BottomHeight = BottomHeight,
                };
                using (var waveStream = new AudioFileReader(FilePath))
                {
                    var waveRenderer = new WaveRender();
                    ItemsSource = waveRenderer.Render(waveStream, PeakProvider, rendererSettings);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("音波图绘制出现异常：" + e.Message);
            }
        }
    }
}
