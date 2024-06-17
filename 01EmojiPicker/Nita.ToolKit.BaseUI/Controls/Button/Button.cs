using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Nita.ToolKit.BaseUI.Controls.Button
{
    /// <summary>
    /// 除了基本的 Button 功能外，该控件还实现了以下用途：
    /// <list type="bullet">
    ///   <item>支持根据SizeType，设置按钮大小</item>
    ///   <item>支持通过Icon，设置按钮图标</item>
    ///   <item>提供点击波浪纹动态效果，可以使用ClickColor设置波纹颜色</item>
    /// </list>
    /// </summary>
    public class Button : System.Windows.Controls.Button
    {
        #region Constructors
        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button),
                new FrameworkPropertyMetadata(typeof(Button)));
        }
        public Button()
        {
            this.Content = "Button";
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
                typeof(SizeType),
                typeof(Button),
                new PropertyMetadata(SizeType.M));
        #endregion

        #region Icon
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Button),
                new PropertyMetadata(OnIconChanged));
        #endregion

        #region ClickColor
        public Brush ClickColor
        {
            get { return (Brush)GetValue(ClickColorProperty); }
            set { SetValue(ClickColorProperty, value); }
        }

        public static readonly DependencyProperty ClickColorProperty =
            DependencyProperty.Register("ClickColor", typeof(Brush), typeof(Button), new PropertyMetadata());
        #endregion

        #region ButtonMode
        public ContentMode ButtonMode
        {
            get { return (ContentMode)GetValue(ContentModeProperty); }
            set { SetValue(ContentModeProperty, value); }
        }

        public static readonly DependencyProperty ContentModeProperty =
            DependencyProperty.Register("ContentMode", typeof(ContentMode), typeof(Button), new PropertyMetadata(ContentMode.IconAndText));
        #endregion

        #region NoBorder
        public bool NoBorder
        {
            get { return (bool)GetValue(NoBorderProperty); }
            set { SetValue(NoBorderProperty, value); }
        }
        public static readonly DependencyProperty NoBorderProperty =
            DependencyProperty.Register("NoBorder", typeof(bool), typeof(Button), new PropertyMetadata(false, OnNoBorderChanged));
        #endregion

        #region 鼠标悬浮样式 MouseOverStyle
        /// <summary>
        /// 鼠标悬浮样式
        /// </summary>
        public static readonly DependencyProperty MouseOverStyleProperty =
            DependencyProperty.Register("MouseOverStyle",
                typeof(MouseOverStyle),
                typeof(Button),
                new PropertyMetadata(MouseOverStyle.ZoomAndChangeColor));
        public MouseOverStyle MouseOverStyle
        {
            get { return (MouseOverStyle)GetValue(MouseOverStyleProperty); }
            set { SetValue(MouseOverStyleProperty, value); }
        }
        #endregion

        #region BottonStyle
        public ButtonStyle ButtonStyle
        {
            get { return (ButtonStyle)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(ButtonStyle), typeof(Button), new PropertyMetadata(ButtonStyle.Default));
        #endregion


        #endregion

        #region 私有方法
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSource newIcon = (ImageSource)e.NewValue;
            var button = (Button)d;
            if (button.ClickColor == null)
            {
                DrawingImage resource = (DrawingImage)d.GetValue(IconProperty);
                button.ClickColor = ColorHelper.GetDominantColorBrush(resource, 1);
            }
        }
        private static void OnNoBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            /*bool NoBorder = (bool)e.NewValue;
            var button = (Button)d;
            DrawingImage resource = (DrawingImage)d.GetValue(IconProperty);
            if (NoBorder)
            {
                button.ClickColor = Brushes.Red;
                //button.ClickColor = ColorHelper.GetDominantColorBrush(resource, 2);
            }
            else
            {
                button.ClickColor = ColorHelper.GetDominantColorBrush(resource, 1);
            }*/
        }
        #endregion

    }
}
