using System.Windows.Controls;
using System.Windows;
using Nita.ToolKit.NAudio.Entity;
using Nita.ToolKit.NAudio.ViewModel;

namespace Nita.ToolKit.NAudio.Controls.View
{
    public class Wave : ContentControl
    {
        private Canvas canvas;
       
        #region Contructors
        static Wave()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Wave),
                new FrameworkPropertyMetadata(typeof(Wave)));
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            canvas = GetTemplateChild("PART_Canvas") as Canvas;
        }

        #region DependencyProperty

        #region Setting
        public WaveSetting WaveSetting
        {
            get { return (WaveSetting)GetValue(WaveSettingProperty); }
            private set { SetValue(WaveSettingProperty, value); }
        }

        public static readonly DependencyProperty WaveSettingProperty =
            DependencyProperty.Register("WaveSetting", typeof(WaveSetting),
                typeof(Wave), new PropertyMetadata(new WaveSetting()));

        #endregion

        #region PeakCalculationStrategyProperty
        public PeakCalculationStrategy PeakCalculationStrategy
        {
            get { return (PeakCalculationStrategy)GetValue(PeakCalculationStrategyProperty); }
            set { SetValue(PeakCalculationStrategyProperty, value); }
        }

        public static readonly DependencyProperty PeakCalculationStrategyProperty =
            DependencyProperty.Register("PeakCalculationStrategy", typeof(PeakCalculationStrategy),
                typeof(Wave), new PropertyMetadata(PeakCalculationStrategy.Max_Absolute_Value,OnWaveSettingPropChanged));

        #endregion

        #region FilePath
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string),
                typeof(Wave), new PropertyMetadata(OnWaveSettingPropChanged));
        #endregion

        #endregion

        private static void OnWaveSettingPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wave = d as Wave;
            if (wave == null || wave.canvas == null || string.IsNullOrEmpty(wave.FilePath))
            {
                return;
            }
            wave.WaveSetting.PeakCalculationStrategy = wave.PeakCalculationStrategy;
            wave.WaveSetting.FilePath = wave.FilePath;
            wave.WaveSetting.Refresh();
            wave.DrawRectangles();
        }

        private void DrawRectangles()
        {
            if (canvas == null
                || WaveSetting.ItemsSource == null
                || WaveSetting.ItemsSource.Count == 0)
            {
                return;
            }
            canvas.Children.Clear();

            canvas.Width = this.Width;

            foreach (var item in WaveSetting.ItemsSource)
            {
                var rect = new System.Windows.Shapes.Rectangle
                {
                    Width = item.Width,
                    Height = item.Height,
                    Fill = item.Color,
                    RadiusX = item.Width * 0.4, // 设置圆角的水平半径
                    RadiusY = item.Width * 0.4  // 设置圆角的垂直半径
                };

                Canvas.SetLeft(rect, item.Left);
                Canvas.SetTop(rect, item.Top);

                canvas.Children.Add(rect);
            }
        }
    }
}
