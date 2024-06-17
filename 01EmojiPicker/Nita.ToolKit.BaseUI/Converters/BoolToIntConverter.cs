using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Converters
{
    /// <summary>
    /// 将布尔值转换为整数（0或1）。
    /// </summary>
    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1 : 0;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue != 0;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
