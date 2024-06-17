using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Controls.ScrollViewer
{
    public class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        #region Fields
        private Visibility lastVerticalScrollBarVisibility = Visibility.Collapsed;
        private Visibility lastHorizontalScrollBarVisibility = Visibility.Collapsed;
        private bool isShiftPressed = false;
        #endregion

        #region Constructors

        static ScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewer),
                new FrameworkPropertyMetadata(typeof(ScrollViewer)));
        }
        #endregion

        #region DependencyProperty

        #region Orientation
        #endregion

        #endregion

        #region Override方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ScrollChanged += ScrollViewer_ScrollChanged;
            this.PreviewKeyDown += ScrollViewer_PreviewKeyDown;
        }
        #endregion

        #region Events

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double delta = e.Delta;
            double offset = this.VerticalOffset;
            double newOffset = offset - delta * 0.1;
            this.ScrollToVerticalOffset(newOffset);
            e.Handled = true;
        }

        private void ScrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                isShiftPressed = true;
            }
        }


        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var currentVerticalScrollBarVisibility = this.ComputedVerticalScrollBarVisibility;
            var currentHorizontalScrollBarVisibility = this.ComputedHorizontalScrollBarVisibility;

            if (currentVerticalScrollBarVisibility == Visibility.Visible
                && lastVerticalScrollBarVisibility != Visibility.Visible)
            {
                this.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            }
            else if (currentVerticalScrollBarVisibility != Visibility.Visible
                && lastVerticalScrollBarVisibility == Visibility.Visible)
            {
                this.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
            }
            lastVerticalScrollBarVisibility = currentVerticalScrollBarVisibility;

            if (currentHorizontalScrollBarVisibility == Visibility.Visible
        && lastHorizontalScrollBarVisibility != Visibility.Visible)
            {
                this.PreviewMouseWheel += ScrollViewer_PreviewMouseWheelHorizontal;
            }
            else if (currentHorizontalScrollBarVisibility != Visibility.Visible
                && lastHorizontalScrollBarVisibility == Visibility.Visible)
            {
                this.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheelHorizontal;
            }
            lastHorizontalScrollBarVisibility = currentHorizontalScrollBarVisibility;
        }

        private void ScrollViewer_PreviewMouseWheelHorizontal(object sender, MouseWheelEventArgs e)
        {
            if (isShiftPressed)
            {
                double delta = e.Delta;
                double offset = this.HorizontalOffset;
                double newOffset = offset - delta * 0.1;
                this.ScrollToHorizontalOffset(newOffset);
                e.Handled = true;
            }
            isShiftPressed = false;
        }

        #endregion

    }
}
