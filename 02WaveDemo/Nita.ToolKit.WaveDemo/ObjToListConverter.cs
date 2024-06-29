using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nita.ToolKit.WaveDemo
{
    public class ObjToListConverter : IValueConverter
    {
        /// <summary>
        /// 获取枚举中添加的描述特性
        /// </summary>
        /// <param name="enumObj"> 枚举类型</param>
        /// <returns></returns>
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        /// <summary>
        /// 定义静态类字典存储枚举的键值对，确保相同的枚举只存一次
        /// </summary>
        public static Dictionary<Type, object> TempDictionary { get; set; } = new Dictionary<Type, object>();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (TempDictionary.ContainsKey(value as Type))
            {
                return TempDictionary[value as Type];
            }
            ///添加<描述,枚举>的键值对，在Combox中定义选中值类型为枚举，展示值为描述
            Dictionary<object, object> temp = new Dictionary<object, object>();
            if (value is Type)
                foreach (var item in Enum.GetValues(value as Type))
                {
                    Enum myEnum = (Enum)item;
                    string description = GetEnumDescription(myEnum);
                    temp.Add(description, item);
                }
            //第一次添加枚举后，将枚举的类型和集合存到字典内
            TempDictionary.Add(value as Type, temp);
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
