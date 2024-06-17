using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace Nita.ToolKit.BaseUI.Controls.CheckBox
{
    public class CheckBox : System.Windows.Controls.CheckBox
    {
        #region Constructors
        static CheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBox),
                new FrameworkPropertyMetadata(typeof(CheckBox)));
        }
        public CheckBox()
        {
            Icons = new ObservableCollection<ImageSource>();
            GetImages();
        }

        #endregion

        #region DependencyProperty

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }
        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType", typeof(SizeType),
                typeof(CheckBox), new PropertyMetadata(SizeType.M));
        #endregion

        #region Icons
        public ObservableCollection<ImageSource> Icons
        {
            get { return (ObservableCollection<ImageSource>)GetValue(IconsProperty); }
            set { SetValue(IconsProperty, value); }
        }
        public static readonly DependencyProperty IconsProperty =
            DependencyProperty.Register("Icons", typeof(ObservableCollection<ImageSource>), typeof(CheckBox), new PropertyMetadata());
        #endregion

        #endregion

        private void GetImages()
        {
            Icons.Add(ColorHelper.GetResourceByKey("UnCheck"));
            Icons.Add(ColorHelper.GetResourceByKey("Check"));
        }
    }
}
