using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double param = System.Convert.ToDouble(parameter);
                return (double)value * param;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && parameter is double)
            {
                if ((double)parameter != 0)
                {
                    return (double)value / (double)parameter;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }
    }
}
