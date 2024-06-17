using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Reflection;

namespace Nita.ToolKit.BaseUI.Controls.ImageSwitcher
{
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class ImageSwitcher : ContentControl
    {
        #region 字段
        private ImageSource _image;
        private Image _imagePart;
        #endregion

        #region 构造函数
        static ImageSwitcher()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSwitcher), new FrameworkPropertyMetadata(typeof(ImageSwitcher)));
        }
        #endregion

        #region 属性

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType",
                typeof(SizeType), typeof(ImageSwitcher),
                new PropertyMetadata(SizeType.M));
        #endregion

        #region Images
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images",
                typeof(ObservableCollection<ImageSource>),
                typeof(ImageSwitcher),
                new PropertyMetadata(new ObservableCollection<ImageSource>(), OnImagesChanged));
        public ObservableCollection<ImageSource> Images
        {
            get { return (ObservableCollection<ImageSource>)GetValue(ImagesProperty); }
            set{SetValue(ImagesProperty, value);}
        }

        #region Image

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageSwitcher), new PropertyMetadata());
        #endregion

      
        #endregion

        #region IsRandom 是否随机切换
        public static readonly DependencyProperty IsRandomProperty =
            DependencyProperty.Register("IsRandom", typeof(bool), typeof(ImageSwitcher), new PropertyMetadata(false));
        public bool IsRandom
        {
            get { return (bool)GetValue(IsRandomProperty); }
            set { SetValue(IsRandomProperty, value); }
        }
        #endregion

        #region ChangeEffect 图片切换模式 
        public static readonly DependencyProperty ChangeEffectProperty =
            DependencyProperty.Register("ChangeEffect",
                typeof(ChangeEffectEnum),
                typeof(ImageSwitcher),
                new PropertyMetadata(ChangeEffectEnum.Loop));

        public ChangeEffectEnum ChangeEffect
        {
            get { return (ChangeEffectEnum)GetValue(ChangeEffectProperty); }
            set { SetValue(ChangeEffectProperty, value); }
        }
        #endregion

        #region ImageIndex 当前图片索引 
        public static readonly DependencyProperty ImageIndexProperty =
            DependencyProperty.Register("ImageIndex",
                typeof(int),
                typeof(ImageSwitcher),
                new PropertyMetadata(0, OnImageIndex));
        public int ImageIndex
        {
            get{return (int)GetValue(ImageIndexProperty);}
            set{ SetValue(ImageIndexProperty, value);}
        }
        #endregion

        #endregion

        #region 事件
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _imagePart = GetTemplateChild("PART_Image") as Image;
            if (_imagePart == null)
            {
                throw new Exception("嘿！ PART_Image从模板中丢失，或者不是Image。抱歉，但是需要此Image。");
            }
            this._imagePart.MouseLeftButtonDown += _imagePart_MouseLeftButtonDown;
        }

        private void _imagePart_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SwitchToNextImage();
        }

        /// <summary>
        /// Images值发生改变后
        /// 为默认展示图片赋值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnImagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSwitcher target = (ImageSwitcher)d;
            if (target == null) return;

            var images = e.NewValue as ObservableCollection<ImageSource>;
            var image = images?.FirstOrDefault();
            if (image == null) return;

            target.Image = image;
            target.ImageIndex = 0;
        }

        private static void OnImageIndex(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSwitcher target = (ImageSwitcher)d;
            if (target == null) return;
            int cIndex = (int)e.NewValue;
            if (target.ImageIndex < target.Images.Count())
            {
                target.Image = target.Images[cIndex];
                target.ImageIndex = cIndex;
            }
        }
        #endregion

        #region 私有方法
        private void SwitchToNextImage()
        {
            if (Images == null || Images.Count == 0)
                return;

            int nextIndex = 0;
            if (IsRandom)
            {
                Random random = new Random();
                do
                {
                    nextIndex = random.Next(Images.Count);
                } while (nextIndex == ImageIndex);
            }
            else
            {
                nextIndex = ImageIndex + 1;
                if (nextIndex >= Images.Count)
                {
                    if (ChangeEffect == ChangeEffectEnum.Loop)
                        nextIndex = 0;
                    else
                        return;
                }
            }
            SetCurrentValue(ImageProperty, Images[nextIndex]);
            SetCurrentValue(ImageIndexProperty, nextIndex);
        }
        #endregion

    }
}
