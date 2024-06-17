using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class HorizontalDirectionToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is HorizontalDirection)
            {
                var direction = (HorizontalDirection)value;
                var parameterString = parameter as string;
                if (!string.IsNullOrEmpty(parameterString))
                {
                    switch (parameterString)
                    {
                        case "Left":
                            return direction == HorizontalDirection.Left? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                        case "Right":
                            return direction == HorizontalDirection.Right? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                        default:
                            return System.Windows.Visibility.Collapsed;
                    }
                }
            }
            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
