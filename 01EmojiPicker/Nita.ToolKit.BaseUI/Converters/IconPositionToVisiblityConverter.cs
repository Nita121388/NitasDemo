using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class IconPositionToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed; // 如果Model为null，返回不可见状态
            }
            if (value is IconPosition mode)
            {
                string param = parameter as string;

                if (param == "Left")
                {
                    return mode == IconPosition.Left ? 
                        Visibility.Visible : Visibility.Collapsed;
                }
                else if (param == "Right")
                {
                    return mode == IconPosition.Right ? 
                        Visibility.Visible : Visibility.Collapsed;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
