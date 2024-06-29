using System;
using System.Collections.ObjectModel;
using System.Drawing;
using NAudio.Wave;
using Nita.ToolKit.NAudio.ViewModel;

namespace Nita.ToolKit.NAudio.Controls.View
{
    public class WaveFormRenderer
    {
        public Image Render(WaveStream waveStream, WaveFormRendererSettings settings)
        {
            return Render(waveStream, new MaxPeakProvider(), settings);
        }

        public Image Render(WaveStream waveStream, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            // 计算每个样本占据的字节数
            int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);

            // 计算总的音频样本数
            var samples = waveStream.Length / (bytesPerSample);

            // 计算每个像素点对应的音频样本数
            var samplesPerPixel = (int)(samples / settings.Width);

            // 计算步长，包括峰值像素和间隔像素
            var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;

            // 初始化峰值提供者，根据音频流和计算的样本数目
            peakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel * stepSize);

            // 调用重载的 Render 方法进行实际渲染
            return Render(peakProvider, settings);
        }

        private static Image Render(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            // 如果设置为使用分贝缩放，则创建 DecibelPeakProvider 来处理峰值
            if (settings.DecibelScale)
                peakProvider = new DecibelPeakProvider(peakProvider, 48);

            // 创建位图对象，宽度为 settings.Width，高度为 settings.TopHeight + settings.BottomHeight
            var b = new Bitmap(settings.Width, settings.TopHeight + settings.BottomHeight);

            // 如果设置了背景色为透明，则将位图设置为透明
            if (settings.BackgroundColor == Color.Transparent)
            {
                b.MakeTransparent();
            }

            // 使用 Graphics 对象绘制位图
            using (var g = Graphics.FromImage(b))
            {
                // 填充背景色
                g.FillRectangle(settings.BackgroundBrush, 0, 0, b.Width, b.Height);

                // 计算中点位置，用于确定绘制的位置
                var midPoint = settings.TopHeight;

                int x = 0;
                var currentPeak = peakProvider.GetNextPeak();

                // 在整个宽度内循环绘制音波图
                while (x < settings.Width)
                {
                    // 获取下一个峰值
                    var nextPeak = peakProvider.GetNextPeak();

                    // 绘制峰值条
                    for (int n = 0; n < settings.PixelsPerPeak; n++)
                    {
                        // 计算并绘制顶部峰值条
                        var lineHeight = settings.TopHeight * currentPeak.Max;
                        g.DrawLine(settings.TopPeakPen, x, midPoint, x, midPoint - lineHeight);

                        // 计算并绘制底部峰值条
                        lineHeight = settings.BottomHeight * currentPeak.Min;
                        g.DrawLine(settings.BottomPeakPen, x, midPoint, x, midPoint - lineHeight);
                        x++;
                    }

                    // 绘制间隔条
                    for (int n = 0; n < settings.SpacerPixels; n++)
                    {
                        // spacer bars are always the lower of the 
                        // 计算最大和最小峰值，并绘制顶部间隔条
                        var max = Math.Min(currentPeak.Max, nextPeak.Max);
                        var min = Math.Max(currentPeak.Min, nextPeak.Min);

                        var lineHeight = settings.TopHeight * max;
                        g.DrawLine(settings.TopSpacerPen, x, midPoint, x, midPoint - lineHeight);

                        // 计算并绘制底部间隔条
                        lineHeight = settings.BottomHeight * min;
                        g.DrawLine(settings.BottomSpacerPen, x, midPoint, x, midPoint - lineHeight);
                        x++;
                    }

                    // 更新当前峰值为下一个峰值，继续下一轮绘制
                    currentPeak = nextPeak;
                }
            }

            // 返回绘制好的位图对象
            return b;
        }

    }
}
