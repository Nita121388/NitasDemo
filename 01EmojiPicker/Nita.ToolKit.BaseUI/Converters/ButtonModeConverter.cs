using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Nita.ToolKit.BaseUI.Entity;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class ButtonModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed; // 如果Model为null，返回不可见状态
            }
            if (value is ContentMode mode)
            {
                string param = parameter as string;

                if (param == "Icon")
                {
                    return mode == ContentMode.IconOnly 
                        || mode == ContentMode.IconAndText 
                        ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (param == "Color")
                {
                    return mode == ContentMode.ColorOnly 
                        || mode == ContentMode.ColorAndText 
                        ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (param == "Text")
                {
                    return mode == ContentMode.TextOnly 
                        || mode == ContentMode.IconAndText
                        || mode == ContentMode.ColorAndText
                        ? Visibility.Visible : Visibility.Collapsed;
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
