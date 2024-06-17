using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Util
{
    public class ResourceHelper
    {
        public static ResourceDictionary _resourceDictionary;
        public static List<ResourceDictionary> _resourceDictionaryList;
        public static string _dictionaryPath = "pack://application:,,,/Nita.ToolKit.BaseUI;component/Themes/Generic.xaml";
        public static string _baseDictionaryPath = "pack://application:,,,/Nita.ToolKit.BaseUI;component/Style/Base.xaml";

        static ResourceHelper()
        {
            _resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(_dictionaryPath, UriKind.Absolute)
            };
            _resourceDictionaryList = new List<ResourceDictionary> { _resourceDictionary };
            _resourceDictionaryList.Add(new ResourceDictionary
            {
                Source = new Uri(_baseDictionaryPath, UriKind.Absolute)
            });
        }
        public static object GetResourceValue(string resourceKey)
        {
            foreach (var resourceDictionary in _resourceDictionaryList)
            {
                if (resourceDictionary.Contains(resourceKey))
                {
                    var resourceValue = resourceDictionary[resourceKey];
                    return resourceValue;
                }
            }
            return null;
            /*if (_resourceDictionary != null && _resourceDictionary.Contains(resourceKey))
            {
                var resourceValue = _resourceDictionary[resourceKey];
                return resourceValue;
            }
            else
            {
                return null;
            }*/
        }
    }

}
