using NAudio.Wave;
using Nita.ToolKit.NAudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nita.ToolKit.NAudio.Controls.View
{
    public class WaveRender
    {
        public ObservableCollection<WaveformItem> Render(WaveStream waveStream, WaveFormRendererSettings settings)
        {
            return Render(waveStream, new MaxPeakProvider(), settings);
        }
        public ObservableCollection<WaveformItem> Render(WaveStream waveStream, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            ObservableCollection<WaveformItem> waveformItems = new ObservableCollection<WaveformItem>();


            // 计算每个样本占据的字节数
            int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);

            // 计算总的音频样本数
            var samples = waveStream.Length / (bytesPerSample);

            // 计算每个像素点对应的音频样本数
            var samplesPerPixel = (int)(samples / settings.Width);

            // 计算步长，包括峰值像素和间隔像素
            var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;

            // 初始化peakProvider，根据音频流和计算的样本数目
            peakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel * stepSize);

            // 调用重载的 Render 方法进行实际渲染
            waveformItems = Render(peakProvider, settings);

            return waveformItems;
        }

        private static ObservableCollection<WaveformItem> Render(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {

            ObservableCollection<WaveformItem> waveformItems = new ObservableCollection<WaveformItem>();


            // 如果设置为使用分贝缩放，则创建 DecibelPeakProvider 来处理峰值
            if (settings.DecibelScale)
                peakProvider = new DecibelPeakProvider(peakProvider, 48);

            int x = 0;
            var currentPeak = peakProvider.GetNextPeak();
            var midPoint = settings.TopHeight;

            int i = 0;
            // 在整个宽度内循环绘制音波图
            while (x < settings.Width)
            {
                // 获取下一个峰值
                var nextPeak = peakProvider.GetNextPeak();


                // 绘制峰值条
                {
                    // 计算并绘制顶底部峰值条
                    var topLineHeight = settings.TopHeight * currentPeak.Max;

                    // 计算并绘制底部峰值条
                    var bottomLineHeight = settings.BottomHeight * currentPeak.Min;

                    var waveHeight = Math.Abs(topLineHeight + bottomLineHeight);

                    var waveformItem = new WaveformItem
                    {
                        Width = settings.PixelsPerPeak,
                        Left = i * (settings.PixelsPerPeak + settings.SpacerPixels),
                        Top =(settings.Height - waveHeight) /2,
                        Height = waveHeight,
                        Color = settings.WaveBrush
                    };
                    waveformItems.Add(waveformItem);
                }

                // 绘制间隔条
                {
                    // 计算最大和最小峰值，并绘制顶部间隔条
                    var max = Math.Min(currentPeak.Max, nextPeak.Max);
                    var min = Math.Max(currentPeak.Min, nextPeak.Min);

                    // 绘制峰值条
                    // 计算并绘制顶底部峰值条
                    var topLineHeight = settings.TopHeight * currentPeak.Max;

                    // 计算并绘制底部峰值条
                    var bottomLineHeight = settings.BottomHeight * currentPeak.Min;


                    var waveHeight = Math.Abs(topLineHeight + bottomLineHeight);


                    var waveformItem = new WaveformItem
                    {
                        Width = settings.SpacerPixels,
                        Left = settings.PixelsPerPeak + i * (settings.PixelsPerPeak + settings.SpacerPixels),
                        Top = (settings.Height - waveHeight) / 2,
                        Height = waveHeight,
                        Color = settings.SpacerBrush
                    };
                    waveformItems.Add(waveformItem);
                }

                x += settings.PixelsPerPeak + settings.SpacerPixels;
                i++;
                currentPeak = nextPeak;
            }

            return waveformItems;
            
        }
    }
}
