using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nita.ToolKit.BaseUI.Controls.Icon
{
    public class Icon : Image
    {
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon),
                new FrameworkPropertyMetadata(typeof(Icon)));
        }

        #region 属性

        #region SizeType

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType",
                typeof(SizeType),
                typeof(Icon), new PropertyMetadata(SizeType.M));

        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        #endregion

        #endregion
    }
}
