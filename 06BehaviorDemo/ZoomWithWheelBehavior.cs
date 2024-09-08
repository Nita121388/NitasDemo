using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BehaviorDemo
{
    public class ZoomWithWheelBehavior : Behavior<FrameworkElement>
    {
        private ScaleTransform _scaleTransform = new ScaleTransform();
        private Point _origin;
        private Point _start;

        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register("ScaleFactor", typeof(double), typeof(ZoomWithWheelBehavior), new PropertyMetadata(1.0));

        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        public static readonly DependencyProperty MinScaleProperty =
            DependencyProperty.Register("MinScale", typeof(double), typeof(ZoomWithWheelBehavior), new PropertyMetadata(0.1));

        public double MinScale
        {
            get { return (double)GetValue(MinScaleProperty); }
            set { SetValue(MinScaleProperty, value); }
        }

        public static readonly DependencyProperty MaxScaleProperty =
            DependencyProperty.Register("MaxScale", typeof(double), typeof(ZoomWithWheelBehavior), new PropertyMetadata(10.0));

        public double MaxScale
        {
            get { return (double)GetValue(MaxScaleProperty); }
            set { SetValue(MaxScaleProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(_scaleTransform);
            AssociatedObject.RenderTransform = transformGroup;
            AssociatedObject.MouseWheel += OnMouseWheel;
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= OnMouseWheel;
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _start = e.GetPosition(AssociatedObject);
            _origin = new Point(_scaleTransform.ScaleX, _scaleTransform.ScaleY);
            AssociatedObject.CaptureMouse();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? ScaleFactor : -ScaleFactor;
            double newScaleX = _scaleTransform.ScaleX + zoom;
            double newScaleY = _scaleTransform.ScaleY + zoom;

            if (newScaleX >= MinScale && newScaleX <= MaxScale && newScaleY >= MinScale && newScaleY <= MaxScale)
            {
                _scaleTransform.ScaleX = newScaleX;
                _scaleTransform.ScaleY = newScaleY;
            }
        }
    }
}
