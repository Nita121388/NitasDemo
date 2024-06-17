using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Nita.ToolKit.BaseUI.Util;
using Nita.ToolKit.BaseUI.Entity;

namespace Nita.ToolKit.BaseUI.Controls.TabControl
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        #region Constructors
        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TabItem),
                new FrameworkPropertyMetadata(typeof(TabItem)));
        }
        #endregion

        #region DependencyProperty

        #region Icon
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(ImageSource),
                typeof(TabItem),
                new PropertyMetadata(ColorHelper.GetResourceByKey("Check")));
        #endregion

        #region HeaderText
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string),
                typeof(TabItem), new PropertyMetadata());
        #endregion

        #region HeaderSizeType
        public SizeType HeaderSizeType
        {
            get { return (SizeType)GetValue(HeaderSizeTypeProperty); }
            set { SetValue(HeaderSizeTypeProperty, value); }
        }

        public static readonly DependencyProperty HeaderSizeTypeProperty =
            DependencyProperty.Register("HeaderSizeType",
                typeof(SizeType), typeof(TabItem), new PropertyMetadata(SizeType.M));

        #endregion

        #region HeaderContentMode
        public ContentMode HeaderContentMode
        {
            get { return (ContentMode)GetValue(HeaderContentModeProperty); }
            set { SetValue(HeaderContentModeProperty, value); }
        }
        public static readonly DependencyProperty HeaderContentModeProperty =
            DependencyProperty.Register("HeaderContentMode",
                typeof(ContentMode),
                typeof(TabItem),
                new PropertyMetadata(ContentMode.IconAndText));
        #endregion

        #endregion
    }
}
