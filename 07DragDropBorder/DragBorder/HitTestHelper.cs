using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DragBorder
{
    internal class HitTestHelper
    {
        public static DragBorderControl GetHitDropControls(Visual visual, Point elementPosition)
        {
            DragBorderControl hitControl = null;

            // 定义命中测试回调方法
            HitTestResultCallback resultCallback = new HitTestResultCallback(result =>
            {
                // 检查命中的控件是否为 DragBorderControl 类型
                if (result.VisualHit is FrameworkElement frameworkElement)
                {
                    var parent = frameworkElement;
                    while (parent != null)
                    {
                        if (parent is DragBorderControl hitItem && !hitItem.ViewModel.IsDragSource)
                        {
                            hitControl = hitItem;
                            return HitTestResultBehavior.Stop;
                        }
                        parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                    }
                }
                return HitTestResultBehavior.Continue;
            });

            // 执行命中测试
            VisualTreeHelper.HitTest(visual, null, resultCallback, new PointHitTestParameters(elementPosition));

            return hitControl;
        }

    }
}
