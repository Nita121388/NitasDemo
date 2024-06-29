using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nita.ToolKit.NAudio.Controls.View
{

    /// <summary>
    /// ������Ⱦ�������࣬�������ò�����Ⱦ���Ĳ�����
    /// </summary>
    public class WaveFormRendererSettings
    {
        /// <summary>
        /// ���캯������ʼ��Ĭ�����á�
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
        /// ��������ʾ���������ԡ�
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ���εĿ�ȡ�
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// ���εĸ߶ȡ�
        /// </summary>
        public int Height { get { return TopHeight + BottomHeight; } }

        /// <summary>
        /// �������εĸ߶ȡ�
        /// </summary>
        public int TopHeight { get; set; }

        /// <summary>
        /// �ײ����εĸ߶ȡ�
        /// </summary>
        public int BottomHeight { get; set; }

        /// <summary>
        /// ÿ����ֵ����������
        /// </summary>
        public int PixelsPerPeak { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        public int SpacerPixels { get; set; }

        /// <summary>
        /// ������ֵ���ʡ�
        /// </summary>
        public virtual Pen TopPeakPen { get; set; }

        /// <summary>
        /// ����������ʡ�
        /// </summary>
        public virtual Pen TopSpacerPen { get; set; }

        /// <summary>
        /// �ײ���ֵ���ʡ�
        /// </summary>
        public virtual Pen BottomPeakPen { get; set; }

        /// <summary>
        /// �ײ�������ʡ�
        /// </summary>
        public virtual Pen BottomSpacerPen { get; set; }

        /// <summary>
        /// �Ƿ�ʹ�÷ֱ��̶ȡ�
        /// </summary>
        public bool DecibelScale { get; set; }

        /// <summary>
        /// ������ɫ��
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// ����ͼƬ��
        /// </summary>
        public Image BackgroundImage { get; set; }
        /// <summary>
        /// ������ɫ
        /// </summary>
        public System.Windows.Media.Brush WaveBrush { get; set; }
        /// <summary>
        /// �����ɫ
        /// </summary>
        public System.Windows.Media.Brush SpacerBrush { get; set; }

        /// <summary>
        /// ������ˢ�����ݱ���ͼƬ����ɫ���ɡ�
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
        /// ���ݸ߶ȡ���ʼ��ɫ�ͽ�����ɫ�������仭�ʡ�
        /// </summary>
        /// <param name="height">���ʸ߶�</param>
        /// <param name="startColor">��ʼ��ɫ</param>
        /// <param name="endColor">������ɫ</param>
        /// <returns>���仭��</returns>
        protected static Pen CreateGradientPen(int height, Color startColor, Color endColor)
        {
            var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, height), startColor, endColor);
            return new Pen(brush);
        }
    }
}