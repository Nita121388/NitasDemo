using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using swc = System.Windows.Controls;

namespace Nita.ToolKit.BaseUI.Controls.Border
{
    /// <summary>
    /// 除了基本的 Border 功能外，该控件还实现了以下用途：
    /// <list type="bullet">
    ///   <item>设置默认样式。</item>
    ///   <item>提供鼠标悬浮样式：放大和改变颜色。</item>
    ///   <item>提供默认圆角样式。</item>
    /// </list>
    /// </summary>
    public class Border : swc.Border
    {
        #region Constructors
        static Border()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Border),
                new FrameworkPropertyMetadata(typeof(Border)));
        }
        #endregion

        #region DependencyProperty

        #region 鼠标悬浮背景颜色  MouseOverBackground
        /// <summary>
        /// 鼠标悬浮背景颜色
        /// </summary>
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(
                "MouseOverBackground",
                typeof(Brush),
                typeof(Border),
                new PropertyMetadata((Brush)Application.Current.Resources["DefaultMouseOverBackground"]));

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        #endregion

        #region 鼠标悬浮样式 MouseOverStyle
        /// <summary>
        /// 鼠标悬浮样式
        /// </summary>
        public static readonly DependencyProperty MouseOverStyleProperty =
            DependencyProperty.Register("MouseOverStyle",
                typeof(MouseOverStyle),
                typeof(Border),
                new PropertyMetadata(MouseOverStyle.ZoomAndChangeColor));
        public MouseOverStyle MouseOverStyle
        {
            get { return (MouseOverStyle)GetValue(MouseOverStyleProperty); }
            set { SetValue(MouseOverStyleProperty, value); }
        }
        #endregion

        #endregion
    }
}
