using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nita.ToolKit.BaseUI.Converters
{
    public class ColorConverterHelper
    {
        public static Brush ConvertStringToBrush(string colorString)
        {
            if (colorString == null || colorString.Length != 7 || colorString[0] != '#')
            {
                throw new ArgumentException("Invalid color string format.");
            }

            byte r = Byte.Parse(colorString.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            byte g = Byte.Parse(colorString.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            byte b = Byte.Parse(colorString.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
    }
}
