using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Controls.ToolTip
{
    /// <summary>
    /// 
    /// </summary>
    public class ToolTip : System.Windows.Controls.ToolTip
    {
        static ToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
        }


    }
}
