using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Controls.Ellipse
{
    public class Ellipse : ContentControl
    {
        #region Contructor
        static Ellipse()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ellipse),
                new FrameworkPropertyMetadata(typeof(Ellipse)));
        }
        #endregion

        #region Fields

        #endregion

        #region DependencyProperty

        #region Color

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string),
                typeof(Ellipse),
                new PropertyMetadata("#DCDCDC"));//TODO
        #endregion

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType", typeof(SizeType), typeof(Ellipse), new PropertyMetadata(SizeType.M));
        #endregion

        #endregion
    }
}
