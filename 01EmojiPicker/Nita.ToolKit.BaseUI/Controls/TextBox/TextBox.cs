using CommunityToolkit.Mvvm.Input;
using Nita.ToolKit.BaseUI.Entity;
using Nita.ToolKit.BaseUI.Util;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Controls.TextBox
{
    [TemplatePart(Name = "PART_NitaClickCanvas", Type = typeof(ClickCanvas.ClickCanvas))]
    [TemplatePart(Name = "PART_Text", Type = typeof(System.Windows.Controls.TextBox))]
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Fields
        private ClickCanvas.ClickCanvas _nitaClickCanvas;
        private System.Windows.Controls.TextBox _textBox;
        #endregion

        #region Constructors
        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox),
                new FrameworkPropertyMetadata(typeof(TextBox)));
        }

        public TextBox()
        {
            //SetValue(TextChangedCommandProperty, new RelayCommand(OnTextChanged));
        }

        #endregion

        #region RoutedEvent

        #region Submit

        public event RoutedEventHandler Submit
        {
            add { AddHandler(SubmitEvent, value); }
            remove { RemoveHandler(SubmitEvent, value); }
        }

        public static readonly RoutedEvent SubmitEvent =
            EventManager.RegisterRoutedEvent(
                "Submit",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TextBox));

        #endregion

        #region TextChanged
       /* public event TextChangedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
                "TextChanged",
                RoutingStrategy.Bubble,
                typeof(TextChangedEventHandler),
                typeof(TextBox));*/

        public IRelayCommand TextChangedCommand => (IRelayCommand)GetValue(TextChangedCommandProperty);

        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.Register(
            nameof(TextChangedCommand),
            typeof(IRelayCommand),
            typeof(TextBox),
            new PropertyMetadata(null)
        );

        #endregion

        #endregion

        #region DependencyProperty

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType",
                typeof(SizeType),
                typeof(TextBox),
                new PropertyMetadata(SizeType.M));
        #endregion

        #region Icon
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(ImageSource),
                typeof(TextBox), new PropertyMetadata(OnIconChanged));
        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(TextBox),
                new PropertyMetadata("TextBox"));
        #endregion

        #region 鼠标悬浮样式 MouseOverStyle
        public static readonly DependencyProperty MouseOverStyleProperty =
            DependencyProperty.Register("MouseOverStyle", typeof(MouseOverStyle), typeof(TextBox), new PropertyMetadata(MouseOverStyle.ChangeColor));
        public MouseOverStyle MouseOverStyle
        {
            get { return (MouseOverStyle)GetValue(MouseOverStyleProperty); }
            set { SetValue(MouseOverStyleProperty, value); }
        }
        #endregion

        #region IsReadOnly
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TextBox), new PropertyMetadata(false));
        #endregion

        #region ContentMode
        public ContentMode ContentMode
        {
            get { return (ContentMode)GetValue(ContentModeProperty); }
            set { SetValue(ContentModeProperty, value); }
        }

        public static readonly DependencyProperty ContentModeProperty =
            DependencyProperty.Register("ContentMode", typeof(ContentMode), typeof(TextBox), new PropertyMetadata(ContentMode.IconAndText));
        #endregion

        #region IconPosition
        public IconPosition IconPosition
        {
            get { return (IconPosition)GetValue(IconPositionProperty); }
            set { SetValue(IconPositionProperty, value); }
        }

        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.Register("IconPosition", typeof(IconPosition), typeof(TextBox),
                new PropertyMetadata(IconPosition.Left));
        #endregion

        #region ClickColor
        public Brush ClickColor
        {
            get { return (Brush)GetValue(ClickColorProperty); }
            set { SetValue(ClickColorProperty, value); }
        }

        public static readonly DependencyProperty ClickColorProperty =
            DependencyProperty.Register("ClickColor", typeof(Brush), typeof(TextBox),
                new PropertyMetadata((Brush)Application.Current.Resources["DefaultClickColor"]));
        #endregion

        #endregion

        #region 私有方法
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSource newIcon = (ImageSource)e.NewValue;
            var nitaTextBox = (TextBox)d;

            DrawingImage resource = (DrawingImage)d.GetValue(IconProperty);
            nitaTextBox.ClickColor = ColorHelper.GetDominantColorBrush(resource, 1);
        }

        #endregion

        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _nitaClickCanvas = GetTemplateChild("PART_NitaClickCanvas") as ClickCanvas.ClickCanvas;
            if (_nitaClickCanvas == null)
            {
                throw new Exception("嘿！ PART_NitaClickCanvas从模板中丢失，或者不是NitaClickCanvas。抱歉，但是需要此Grid。");
            }
            _textBox = GetTemplateChild("PART_Text") as System.Windows.Controls.TextBox;
            if (_textBox == null)
            {
                throw new Exception("嘿！ PART_Text 从模板中丢失，或者不是 TextBox。");
            }
            _textBox.PreviewKeyDown += NitaClickCanvas_PreviewKeyDown;
            _textBox.PreviewMouseDown += _textBox_PreviewMouseDown;
            _textBox.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(TextProperty, _textBox.Text);
            RaiseEvent(new TextChangedEventArgs(TextChangedEvent, UndoAction.None));
        }
        private void _textBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _textBox.Focus();
            _textBox.SelectAll();
            e.Handled = true;
        }
        #endregion
        private void NitaClickCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseEvent(new RoutedEventArgs(SubmitEvent, this));

                _nitaClickCanvas.EnableActiveStyle();
            }
        }
        private void OnTextChanged()
        {
            //SetValue(TextProperty, _textBox.Text);
            //RaiseEvent(new TextChangedEventArgs(TextChangedEvent, UndoAction.None));
        }
    }
}