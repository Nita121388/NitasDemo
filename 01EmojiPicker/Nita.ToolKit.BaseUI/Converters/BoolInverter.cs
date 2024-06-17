using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Nita.ToolKit.BaseUI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolInverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool flag))
            {
                return value;
            }

            return !flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool flag))
            {
                return value;
            }

            return !flag;
        }
    }
}
