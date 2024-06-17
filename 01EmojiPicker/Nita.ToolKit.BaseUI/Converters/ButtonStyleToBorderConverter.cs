using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;
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
    public class ButtonStyleToBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ButtonStyle bs)
            {
                if (bs == ButtonStyle.Flat)
                {
                    return ResourceHelper.GetResourceValue("FlatBorder");
                }
                return ResourceHelper.GetResourceValue("DefaultBorder") ;
            }
            else
            {
                throw new ArgumentException("value must be a ButtonStyle");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
