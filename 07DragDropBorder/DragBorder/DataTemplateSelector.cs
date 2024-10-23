using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DragBorder
{
    public class DragBorderDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate IntTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is DataItem dataItem)
            {
                switch (dataItem.Type)
                {
                    case "String":
                        return StringTemplate;
                    case "Int":
                        return IntTemplate;
                    // 可以添加更多类型  
                    default:
                        return base.SelectTemplate(item, container); // 或者返回 null，如果没有匹配的模板  
                }
            }
            return base.SelectTemplate(item, container); // 或者返回 null，如果 item 不是 DataItem 或为 null  
        }
    }
}
