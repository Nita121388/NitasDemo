using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nita.ToolKit.NAudio.Controls.View
{

    /// <summary>
    /// 波形渲染器设置类，用于配置波形渲染器的参数。
    /// </summary>
    public class WaveFormRendererSettings
    {
        /// <summary>
        /// 构造函数，初始化默认设置。
        /// </summary>
        public WaveFormRendererSettings()
        {
            Width = 800;
            TopHeight = 50;
            BottomHeight = 50;
            PixelsPerPeak = 1;
            SpacerPixels = 0;
            BackgroundColor = Color.Beige;
        }

        /// <summary>
        /// 仅用于显示的名称属性。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 波形的宽度。
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 波形的高度。
        /// </summary>
        public int Height { get { return TopHeight + BottomHeight; } }

        /// <summary>
        /// 顶部波形的高度。
        /// </summary>
        public int TopHeight { get; set; }

        /// <summary>
        /// 底部波形的高度。
        /// </summary>
        public int BottomHeight { get; set; }

        /// <summary>
        /// 每个峰值的像素数。
        /// </summary>
        public int PixelsPerPeak { get; set; }

        /// <summary>
        /// 间隔像素数。
        /// </summary>
        public int SpacerPixels { get; set; }

        /// <summary>
        /// 顶部峰值画笔。
        /// </summary>
        public virtual Pen TopPeakPen { get; set; }

        /// <summary>
        /// 顶部间隔画笔。
        /// </summary>
        public virtual Pen TopSpacerPen { get; set; }

        /// <summary>
        /// 底部峰值画笔。
        /// </summary>
        public virtual Pen BottomPeakPen { get; set; }

        /// <summary>
        /// 底部间隔画笔。
        /// </summary>
        public virtual Pen BottomSpacerPen { get; set; }

        /// <summary>
        /// 是否使用分贝刻度。
        /// </summary>
        public bool DecibelScale { get; set; }

        /// <summary>
        /// 背景颜色。
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// 背景图片。
        /// </summary>
        public Image BackgroundImage { get; set; }
        /// <summary>
        /// 音波颜色
        /// </summary>
        public System.Windows.Media.Brush WaveBrush { get; set; }
        /// <summary>
        /// 间隔颜色
        /// </summary>
        public System.Windows.Media.Brush SpacerBrush { get; set; }

        /// <summary>
        /// 背景画刷，根据背景图片或颜色生成。
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (BackgroundImage == null) return new SolidBrush(BackgroundColor);
                return new TextureBrush(BackgroundImage, WrapMode.Clamp);
            }
        }

        /// <summary>
        /// 根据高度、起始颜色和结束颜色创建渐变画笔。
        /// </summary>
        /// <param name="height">画笔高度</param>
        /// <param name="startColor">起始颜色</param>
        /// <param name="endColor">结束颜色</param>
        /// <returns>渐变画笔</returns>
        protected static Pen CreateGradientPen(int height, Color startColor, Color endColor)
        {
            var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, height), startColor, endColor);
            return new Pen(brush);
        }
    }
}