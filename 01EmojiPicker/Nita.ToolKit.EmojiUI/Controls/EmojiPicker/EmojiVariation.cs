using CommunityToolkit.Mvvm.Input;
using Nita.ToolKit.Emoji.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SysControls = System.Windows.Controls;
using SysPrim = System.Windows.Controls.Primitives;


namespace Nita.ToolKit.EmojiUI.Controls.EmojiPicker
{
    [TemplatePart(Name = "PART_VariationPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_VariationListView", Type = typeof(ListView))]
    public class EmojiVariation : ContentControl
    {
        private Popup _variationPopup;
        private ToggleButton _originalEmojiButton;

        #region OnApplyTemplate
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _variationPopup = GetTemplateChild<Popup>("PART_VariationPopup");
            _variationPopup.Opened += _variationPopup_Opened;
            _variationPopup.Closed += _variationPopup_Closed;
            _variationPopup.LostFocus += _variationPopup_LostFocus;
        }


        #region _variationPopup

        private void _variationPopup_Opened(object? sender, EventArgs e)
        {
            try
            {

                if (sender is not Popup popup) return;
                if (!_variationPopup.IsOpen) return;

                var child = popup.Child;
                IInputElement old_focus = null;
                child.Focusable = true;
                child.IsVisibleChanged += (o, ea) =>
                {
                    if (child.IsVisible)
                    {
                        old_focus = Keyboard.FocusedElement;
                        Keyboard.Focus(child);
                    }
                };

                popup.PreviewMouseUp += (sender, e) =>
                {
                    if (e.Source is not Button)
                    {
                        e.Handled = true;
                        return;
                    }
                };
            }
            catch (Exception ex)
            {

            }

        }

        private void _variationPopup_Closed(object? sender, EventArgs e)
        {
            //Keyboard.Focus(Keyboard.FocusedElement);
            if (_originalEmojiButton != null) _originalEmojiButton.IsChecked = false;
            IsOpen = false;
        }

        private void _variationPopup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source is not Button)
            {
                e.Handled = true;
                return;
            }

            if (_originalEmojiButton != null) _originalEmojiButton.IsChecked = false;
            IsOpen = false;
        }

        #endregion
        #endregion

        #region Contructors
        static EmojiVariation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiVariation),
                new FrameworkPropertyMetadata(typeof(EmojiVariation)));
        }

        public EmojiVariation()
        {
            SetValue(EmojiPickedCommandProperty, new RelayCommand<Button>(OnEmojiPicked));
            SetValue(VPopupOpenedCommandProperty, new RelayCommand<ToggleButton>(OnVPopupOpened));
        }

        private void OnVPopupOpened(ToggleButton? button)
        {
            _originalEmojiButton = button;
        }
        #endregion

        #region Support  Events

        #region Picked

        public static readonly RoutedEvent PickedEvent = EventManager.RegisterRoutedEvent(
            nameof(Picked),
            RoutingStrategy.Bubble,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiVariation)
        );

        /// <summary>
        ///  选择Emoji变体
        /// </summary>
        public event EmojiPickedEventHandler Picked
        {
            add => AddHandler(PickedEvent, value);
            remove => RemoveHandler(PickedEvent, value);
        }
        #endregion

        #region OnIsOpenChanged
        public static readonly RoutedEvent IsOpenChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(OnIsOpenChanged),
            RoutingStrategy.Bubble,
            typeof(IsOpenChangedEventHandler),
            typeof(EmojiVariation)
        );

        /// <summary>
        ///  选择Emoji变体
        /// </summary>
        public event IsOpenChangedEventHandler IsOpenChanged
        {
            add => AddHandler(IsOpenChangedEvent, value);
            remove => RemoveHandler(IsOpenChangedEvent, value);
        }
        #endregion



        #endregion

        #region DependencyProperty

        /* #region IsChecked
         public bool IsChecked
         {
             get { return (bool)GetValue(IsCheckedProperty); }
             set { SetValue(IsCheckedProperty, value); }
         }

         public static readonly DependencyProperty IsCheckedProperty =
             DependencyProperty.Register("IsChecked",
                 typeof(bool),
                 typeof(EmojiVariation),
                 new PropertyMetadata(false));
         #endregion*/

        #region IsOpen
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen",
                typeof(bool),
                typeof(EmojiVariation),
                new PropertyMetadata(false, OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            /* var control = d as EmojiVariation;
             if (control == null) return;
             if (control.IsOpen)
             {
                 control._variationPopup.IsOpen = true;
             }

             control._originalEmojiButton = FindVisualParent<SysPrim.ToggleButton>(d);*/
        }
        #endregion

        #region Selection

        public event PropertyChangedEventHandler SelectionChanged;
        public string Selection
        {
            get => (string)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
         nameof(Selection), typeof(string), typeof(EmojiVariation),
             new FrameworkPropertyMetadata("☺", OnSelectionPropertyChanged));

        private static void OnSelectionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EmojiVariation)?.OnSelectionChanged(e.NewValue as string);
        }

        private void OnSelectionChanged(string s)
        {
            var is_disabled = string.IsNullOrEmpty(s);
            SelectionChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selection)));
        }
        #endregion



        public static readonly DependencyProperty EmojiPickedCommandProperty = DependencyProperty.Register(
            nameof(EmojiPickedCommand),
            typeof(IRelayCommand),
            typeof(EmojiVariation),
            new PropertyMetadata(null)
        );
        public IRelayCommand EmojiPickedCommand => (IRelayCommand)GetValue(EmojiPickedCommandProperty);

        public IRelayCommand VPopupOpenedCommand => (IRelayCommand)GetValue(VPopupOpenedCommandProperty);

        public static readonly DependencyProperty VPopupOpenedCommandProperty = DependencyProperty.Register(
            nameof(VPopupOpenedCommand),
            typeof(IRelayCommand),
            typeof(EmojiVariation),
            new PropertyMetadata(null)
        );


        #endregion

        #region Event
        private void OnEmojiPicked(Button emojiButton)
        {
            if (emojiButton == null) return;

            if (emojiButton.DataContext is Emoji.Data.Emoji emoji)
            {
                var selection = emoji.Text;
                //IsChecked = false; 
                SetValue(IsOpenProperty, false);
                SetValue(SelectionProperty, selection);
                RaiseEvent(new EmojiPickedEventArgs(PickedEvent, emojiButton, Selection));//暂时没有订阅
                RaiseEvent(new IsOpenChangedEventArgs(IsOpenChangedEvent, emojiButton, false));
                //_originalEmojiButton.IsChecked = false;
            }
        }
        #endregion

        #region GetTemplateChild
        public T GetTemplateChild<T>(string childName) where T : class
        {
            T child = GetTemplateChild(childName) as T;
            if (child == null)
            {
                throw new Exception($"Error: {childName} is missing from the template or is not a {typeof(T).Name}. A {typeof(T).Name} is required.");
            }
            return child;
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }
        #endregion

    }
}
