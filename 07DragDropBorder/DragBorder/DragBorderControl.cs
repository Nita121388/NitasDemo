using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace DragBorder
{
    public class DragBorderControl : ContentControl
    {
        static DragBorderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragBorderControl), new FrameworkPropertyMetadata(typeof(DragBorderControl)));
        }

        public DragBorderControl()
        {
            ViewModel = new DragBorderViewModel(this);
            this.PreviewMouseLeftButtonUp += DragBorderControl_PreviewMouseLeftButtonUp;
            this.MouseMove += DragBorderControl_MouseMove;
        }

        private void DragBorderControl_MouseMove(object sender, MouseEventArgs e)
        {
            ViewModel.DragMove(e);
        }

        private void DragBorderControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Drag(e);
        }

        public DragBorderViewModel ViewModel
        {
            get { return (DragBorderViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DragBorderViewModel), typeof(DragBorderControl), new PropertyMetadata(null));

        internal Border PART_Border;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Border = GetTemplateChild("PART_Border") as System.Windows.Controls.Border;
        }

        #region DragData
        public object DragData
        {
            get { return (object)GetValue(DragDataProperty); }
            set { SetValue(DragDataProperty, value); }
        }

        public static readonly DependencyProperty DragDataProperty =
            DependencyProperty.Register("DragData", typeof(object), typeof(DragBorderControl), new PropertyMetadata(null,
                OnDragDataChanged));

        private static void OnDragDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DragBorderControl control = d as DragBorderControl;
            control.ViewModel.DragData = e.NewValue;
        }
        #endregion

        #region FrameworkElement ParentControl
        public FrameworkElement ParentControl
        {
            get { return (FrameworkElement)GetValue(ParentControlProperty); }
            set { SetValue(ParentControlProperty, value); }
        }

        public static readonly DependencyProperty ParentControlProperty =
            DependencyProperty.Register("ParentControl", typeof(FrameworkElement), typeof(DragBorderControl), new PropertyMetadata(null, OnParentControlChanged));

        private static void OnParentControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DragBorderControl control = d as DragBorderControl;
            control.ViewModel.ParentControl = e.NewValue as FrameworkElement;
        }
        #endregion

        #region IsRemoveAfterMerage
        public bool IsRemoveAfterMerage
        {
            get { return (bool)GetValue(IsRemoveAfterMerageProperty); }
            set { SetValue(IsRemoveAfterMerageProperty, value); }   
        }

        public static readonly DependencyProperty IsRemoveAfterMerageProperty =
            DependencyProperty.Register("IsRemoveAfterMerage", typeof(bool), typeof(DragBorderControl), new PropertyMetadata(false, OnIsRemoveAfterMerageChanged));

        private static void OnIsRemoveAfterMerageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DragBorderControl control = d as DragBorderControl;
            control.ViewModel.IsRemoveAfterMerage = (bool)e.NewValue;
        }
        #endregion

    }
}
