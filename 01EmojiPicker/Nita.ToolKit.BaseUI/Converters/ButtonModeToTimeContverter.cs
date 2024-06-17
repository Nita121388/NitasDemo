using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class ButtonModeToTimeContverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ContentMode bm)
            {
                if (bm == ContentMode.IconOnly)
                {
                    return 2;
                }
                return 1.3;
            }
            else
            {
                throw new ArgumentException("value must be a SizeType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
