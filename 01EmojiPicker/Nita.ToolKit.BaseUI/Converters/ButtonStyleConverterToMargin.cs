using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class ButtonStyleConverterToMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var buttonStyle = (ButtonStyle)value;
            switch (buttonStyle)
            {
                case ButtonStyle.Default:
                    return ResourceHelper.GetResourceValue("DefaultMargin");
                case ButtonStyle.Flat:
                    return ResourceHelper.GetResourceValue("FlatMargin");
                default:
                    throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
