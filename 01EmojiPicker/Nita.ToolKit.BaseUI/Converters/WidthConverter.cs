using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double.TryParse(value.ToString(), out double actualWidth);
            Double.TryParse(parameter.ToString(), out double minWidth);
            if (actualWidth < minWidth)
            {
                return minWidth;
            }
            return actualWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
