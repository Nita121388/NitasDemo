using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class SizeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SizeType sizeType)
            {
                string param = parameter as string;
                if (param == null)
                {
                    param = "";
                }
                string resourceName = sizeType.ToString() + param;
                object resource = ResourceHelper.GetResourceValue(resourceName);
                return (double)resource;
            }
            else
            {
                throw new ArgumentException("value must be a SizeType");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                string param = parameter as string;
                if (param == null)
                {
                    throw new ArgumentException("parameter must be a string");
                }
                foreach (SizeType sizeType in Enum.GetValues(typeof(SizeType)))
                {
                    string resourceName = sizeType.ToString() + param;
                    object resource = Application.Current.FindResource(resourceName);
                    if (resource is double resourceValue && resourceValue == doubleValue)
                    {
                        return sizeType;
                    }
                }
                throw new ArgumentException("value must match a SizeType resource");
            }
            else
            {
                throw new ArgumentException("value must be a double");
            }
        }
    }
}
