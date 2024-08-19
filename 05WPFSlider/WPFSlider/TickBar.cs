using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace WPFSlider
{
    public class TickBar : System.Windows.Controls.Primitives.TickBar
    {
        #region CurrentTick
        public static readonly DependencyProperty CurrentTickProperty =
            DependencyProperty.Register("CurrentTick",
                typeof(double),
                typeof(TickBar),
                new UIPropertyMetadata(0.0, OnCurrentTickChanged));


        public double CurrentTick
        {
            get { return (double)GetValue(CurrentTickProperty); }
            set { SetValue(CurrentTickProperty, value); }
        }
        private static void OnCurrentTickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //触发渲染
            if (d is TickBar tickBar)
            {
                tickBar.InvalidateVisual();
            }
        }

        #endregion

        #region TickRenderMode

        public static readonly DependencyProperty TickRenderModeProperty =
            DependencyProperty.Register("TickRenderMode",
                typeof(TickRenderMode),
                typeof(TickBar),
                new UIPropertyMetadata(TickRenderMode.FixedTicksOnMouseOver));

        public TickRenderMode TickRenderMode
        {
            get { return (TickRenderMode)GetValue(TickRenderModeProperty); }
            set { SetValue(TickRenderModeProperty, value); }
        }
        #endregion

        #region PlacementMode
        public PlacementMode PlacementMode
        {
            get { return (PlacementMode)GetValue(PlacementModeProperty); }
            set { SetValue(PlacementModeProperty, value); }
        }

        public static readonly DependencyProperty PlacementModeProperty =
            DependencyProperty.Register("PlacementMode", typeof(PlacementMode),
                typeof(TickBar),
                new PropertyMetadata(PlacementMode.Right));
        #endregion


        #region NitaTickPlacement
        public NitaTickPlacement NitaTickPlacement
        {
            get { return (NitaTickPlacement)GetValue(NitaTickPlacementProperty); }
            set { SetValue(NitaTickPlacementProperty, value); }
        }

        public static readonly DependencyProperty NitaTickPlacementProperty =
            DependencyProperty.Register("NitaTickPlacement",
                typeof(NitaTickPlacement),
                typeof(TickBar),
                new UIPropertyMetadata());
        #endregion
        private const double Radius = 3;
        private const double ShadowRadius = 4;
        private static readonly Brush ShadowBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#adadad"));

        protected override void OnRender(DrawingContext dc)
        {
            if (TickRenderMode == TickRenderMode.Base)
            {
                base.OnRender(dc);
            }
            else
            {
                double tickFrequency = this.TickFrequency;
                if (tickFrequency <= 0)
                    return;

                double minimum = this.Minimum;
                double maximum = this.Maximum;
                double range = maximum - minimum;
                double tickCount = range / tickFrequency;

                double width = this.ActualWidth - 2 * Radius;
                double height = this.ActualHeight;

                for (int i = 0; i <= tickCount; i++)
                {
                    double tickValue = minimum + i * tickFrequency;

                    if (!Ticks.Contains(tickValue) && TickRenderMode == TickRenderMode.FixedTicksOnMouseOver)
                        continue;

                    double x = (tickValue - minimum) / range * width + Radius;
                    double y = height / 2;

                    if (TickRenderMode == TickRenderMode.FixedTicksOnMouseOver)
                    {
                        DrawFixedTicksOnMouseOver(dc, tickValue, x, y);
                    }
                    else if (TickRenderMode == TickRenderMode.AutoShowOnMouseMove &&
                             (NitaTickPlacement == NitaTickPlacement.Top || NitaTickPlacement == NitaTickPlacement.Bottom))
                    {
                        DrawAutoShowOnMouseMove(dc, tickValue, x, y);
                    }
                }
            }
        }

        private void DrawFixedTicksOnMouseOver(DrawingContext dc, double tickValue, double x, double y)
        {
            double pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            FormattedText formattedText = new FormattedText(
                tickValue.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 12, Brushes.Gray, pixelsPerDip);

            double rectWidth = formattedText.Width + 10;
            double rectHeight = formattedText.Height + 4;
            double textY = NitaTickPlacement == NitaTickPlacement.Top ? y - rectHeight - 4 : y + 4;

            dc.DrawText(formattedText, new Point(x - formattedText.Width / 2, textY + 2));

            if (!CurrentTick.Equals(tickValue))
            {
                dc.DrawEllipse(ShadowBrush, null, new Point(x, y), ShadowRadius, ShadowRadius);
                dc.DrawEllipse(Fill, null, new Point(x, y), Radius, Radius);
            }
        }

        private void DrawAutoShowOnMouseMove(DrawingContext dc, double tickValue, double x, double y)
        {
            bool isShowCurrentValue = CurrentTick.Equals(tickValue);
            tickValue = Math.Round(tickValue, 2);

            if (isShowCurrentValue)
            {
                double pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

                FormattedText formattedText = new FormattedText(
                    tickValue.ToString(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"), 12, Brushes.Gray, pixelsPerDip);

                double rectWidth = formattedText.Width + 10;
                double rectHeight = formattedText.Height + 4;
                double textY = NitaTickPlacement == NitaTickPlacement.Top ? y - rectHeight - 10 : y + 10;

                Rect rect = new Rect(new Point(x - rectWidth / 2, textY), new Size(rectWidth, rectHeight));
                dc.DrawRoundedRectangle(ShadowBrush, null, rect, 5, 5);
                dc.DrawRoundedRectangle(Fill, null, new Rect(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2), 5, 5);
                dc.DrawText(formattedText, new Point(x - formattedText.Width / 2, textY + 2));
            }
        }
    }


    //刻度渲染类型
    public enum TickRenderMode
    {
        Base,
        /// <summary>
        /// 鼠标移动时自动展示当前刻度
        /// </summary>
        AutoShowOnMouseMove,

        /// <summary>
        /// 根据设置的刻度显示固定刻度
        /// </summary>
        FixedTicksOnMouseOver,
    }
}
