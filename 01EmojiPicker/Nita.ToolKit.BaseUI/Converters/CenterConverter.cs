using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class CenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double && values[1] is double)
            {
                double canvasWidth = (double)values[0];
                double contentWidth = (double)values[1];
                return (canvasWidth - contentWidth) / 2;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
