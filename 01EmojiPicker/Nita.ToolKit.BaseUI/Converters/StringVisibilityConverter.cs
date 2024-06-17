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
    /// <summary>
    /// Converts a string to a Visibility value based on whether the string is null or empty.
    /// If the parameter is "Inverse", the converter will return Visibility.Collapsed if the string is not null or empty.
    /// </summary>
    public class StringVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInverse = parameter.ToString() == "Inverse";
            bool isString = value is string;
            if (!isInverse && isString || ( isInverse && !isString ))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
