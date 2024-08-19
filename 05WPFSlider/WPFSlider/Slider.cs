using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace WPFSlider
{

    public class Slider : System.Windows.Controls.Slider
    {
        private TickBar _tickBar;
        private Thumb _thumb;

        #region Constructors
        static Slider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Slider),
                new FrameworkPropertyMetadata(typeof(Slider)));
        }
        public Slider()
        {
            this.MouseEnter += Slider_MouseEnter;
            this.MouseLeave += Slider_MouseLeave;
            this.Loaded += Slider_Loaded;
            this.ValueChanged += Slider_ValueChanged;
        }

        #endregion

        #region Properties

        #region IsShowTick

        public static readonly DependencyProperty IsShowTickProperty =
            DependencyProperty.Register("IsShowTick",
                typeof(bool), typeof(Slider), new PropertyMetadata(false));

        public bool IsShowTick
        {
            get { return (bool)GetValue(IsShowTickProperty); }
            set { SetValue(IsShowTickProperty, value); }
        }
        #endregion

        #region TickRenderMode

        public TickRenderMode TickRenderMode
        {
            get { return (TickRenderMode)GetValue(TickRenderModeProperty); }
            set { SetValue(TickRenderModeProperty, value); }
        }

        public static readonly DependencyProperty TickRenderModeProperty =
            DependencyProperty.Register("TickRenderMode",
                typeof(TickRenderMode),
                typeof(Slider),
                new UIPropertyMetadata(TickRenderMode.FixedTicksOnMouseOver));
        public bool IsNeedShowOnMouseOver
        {
            get
            {
                var TickRenderModeStr = TickRenderMode.ToString();
                return TickRenderModeStr.Contains("OnMouseOver");
            }
        }

        public bool IsNeedShowOnMouseMove
        {
            get
            {
                var TickRenderModeStr = TickRenderMode.ToString();
                return TickRenderModeStr.Contains("OnMouseMove");
            }
        }
        #endregion

        #region IsDragging
        public static readonly DependencyProperty IsDraggingProperty =
            DependencyProperty.Register("IsDragging",
                typeof(bool),
                typeof(Slider),
                new UIPropertyMetadata(false, OnIsDraggingChanged));

        private static void OnIsDraggingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as Slider;
            if (slider == null)
                return;

            if (slider.IsDragging && (slider.IsNeedShowOnMouseMove || slider.IsNeedShowOnMouseOver))
            {
                slider.IsShowTick = true;
            }
            else
            {
                slider.IsShowTick = false;
            }
        }

        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }
        #endregion

        #region NitaTickPlacement
        public NitaTickPlacement NitaTickPlacement
        {
            get { return (NitaTickPlacement)GetValue(NitaTickPlacementProperty); }
            set { SetValue(NitaTickPlacementProperty, value); }
        }

        public static readonly DependencyProperty NitaTickPlacementProperty =
            DependencyProperty.Register("NitaTickPlacement",
                typeof(NitaTickPlacement),
                typeof(Slider),
                new UIPropertyMetadata(NitaTickPlacement.Bottom));

        public bool IsShowRightTick
        {
            get
            {
                return NitaTickPlacement == NitaTickPlacement.Right;
            }
        }
        public bool IsShowLeftTick
        {
            get
            {
                return NitaTickPlacement == NitaTickPlacement.Left;
            }
        }
        #endregion

        #region ValueUnit
        public string ValueUnit
        {
            get { return (string)GetValue(ValueUnitProperty); }
            set { SetValue(ValueUnitProperty, value); }
        }

        public static readonly DependencyProperty ValueUnitProperty =
            DependencyProperty.Register("ValueUnit",
                typeof(string),
                typeof(Slider),
                new UIPropertyMetadata(null));

        #endregion
        #region DisplayValue 
        public string DisplayValue
        {
            get { return (string)GetValue(DisplayValueProperty); }
            set { SetValue(DisplayValueProperty, value); }
        }

        public static readonly DependencyProperty DisplayValueProperty =
            DependencyProperty.Register("DisplayValue",
                typeof(string),
                typeof(Slider),
                new UIPropertyMetadata(null));
        #endregion



        #endregion

        #region Event

        private void Slider_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsNeedShowOnMouseOver)
            {
                IsShowTick = true;
            }
        }

        private void Slider_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsNeedShowOnMouseOver)
            {
                IsShowTick = false;
            }
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            this.DisplayValue = Value + ValueUnit;
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.DisplayValue = Value + ValueUnit;
        }
        #region DragCompleted

        public static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent(
            nameof(DragCompleted),
            RoutingStrategy.Bubble,
            typeof(DragCompletedEventHandler),
            typeof(Slider)
        );

        /// <summary>
        /// 暂停播放事件
        /// </summary>
        public event DragCompletedEventHandler DragCompleted
        {
            add => AddHandler(DragCompletedEvent, value);
            remove => RemoveHandler(DragCompletedEvent, value);
        }
        #endregion


        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tickBar = GetTemplateChild("TopTick") as TickBar;
            _thumb = GetTemplateChild("PART_Thumb") as System.Windows.Controls.Primitives.Thumb;
            if (_thumb != null)
            {
                _thumb.PreviewMouseLeftButtonDown += Thumb_MouseLeftButtonDown;
                _thumb.PreviewMouseLeftButtonUp += Thumb_MouseLeftButtonUp;
                _thumb.DragStarted += Thumb_DragStarted;
                _thumb.DragCompleted += Thumb_DragCompleted;
            }
        }
        private void Thumb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsDragging = true;
        }
        private void Thumb_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsDragging = false;
        }
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            IsDragging = true;
        }
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (IsDragging)
            {
                IsDragging = false;
            }
        }
    }
    public enum NitaTickPlacement
    {
        Left,
        Right,
        Top,
        Bottom
    }
}
