using CommunityToolkit.Mvvm.Input;
using Nita.ToolKit.Emoji.Data;
using System.Windows;
using System.Windows.Controls;

namespace Nita.ToolKit.EmojiUI.Controls.EmojiPicker
{
    /// <summary>
    /// Emoji Picker Tool Bar
    /// </summary>
    public class EmojiPickerToolBar : ContentControl
    {
        private BaseUI.Controls.TextBox.TextBox _searchTextBox;

        #region Constructors
        static EmojiPickerToolBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiPickerToolBar), 
                new FrameworkPropertyMetadata(typeof(EmojiPickerToolBar)));
        }
        public EmojiPickerToolBar()
        {
            SetValue(RandomedCommandProperty, new RelayCommand(OnRandomClick));
            SetValue(DeletedCommandProperty, new RelayCommand(OnDeleteClick));
            SetValue(SearchCommandProperty, new RelayCommand<string>(OnSearch));
        }
        #endregion

        #region Command

        #region RandomedCommand
        public IRelayCommand RandomedCommand => (IRelayCommand)GetValue(RandomedCommandProperty);

        public static readonly DependencyProperty RandomedCommandProperty = DependencyProperty.Register(
            nameof(RandomedCommand),
            typeof(IRelayCommand),
            typeof(EmojiPickerToolBar),
            new PropertyMetadata(null)
        );

        public static readonly RoutedEvent RandomedEvent = EventManager.RegisterRoutedEvent(
            nameof(Randomed),
            RoutingStrategy.Direct,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPickerToolBar)
        );

        /// <summary>
        ///  Random Emoji
        /// </summary>
        public event EmojiPickedEventHandler Randomed
        {
            add => AddHandler(RandomedEvent, value);
            remove => RemoveHandler(RandomedEvent, value);
        }
        #endregion

        #region DeletedCommand
        public IRelayCommand DeletedCommand => (IRelayCommand)GetValue(DeletedCommandProperty);

        public static readonly DependencyProperty DeletedCommandProperty = DependencyProperty.Register(
            nameof(DeletedCommand),
            typeof(IRelayCommand),
            typeof(EmojiPickerToolBar),
            new PropertyMetadata(null)
        );

        public static readonly RoutedEvent DeletedEvent = EventManager.RegisterRoutedEvent(
            nameof(Deleted),
            RoutingStrategy.Bubble,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPickerToolBar)
        );

        /// <summary>
        ///  Random Emoji
        /// </summary>
        public event EmojiPickedEventHandler Deleted
        {
            add => AddHandler(DeletedEvent, value);
            remove => RemoveHandler(DeletedEvent, value);
        }
        #endregion

        #region SearchCommand
        public IRelayCommand SearchCommand => (IRelayCommand)GetValue(SearchCommandProperty);

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            nameof(SearchCommand),
            typeof(IRelayCommand),
            typeof(EmojiPickerToolBar),
            new PropertyMetadata(null)
        );

        #endregion
        #endregion

        #region EmojiPickerToolArgs

        public EmojiPickerToolArgs EmojiPickerToolArgs
        {
            get { return (EmojiPickerToolArgs)GetValue(EmojiPickerToolArgsProperty); }
            set { SetValue(EmojiPickerToolArgsProperty, value); }
        }

        public static readonly DependencyProperty EmojiPickerToolArgsProperty =
            DependencyProperty.Register("EmojiPickerToolArgs", typeof(EmojiPickerToolArgs), 
                typeof(EmojiPickerToolBar), new PropertyMetadata());
        #endregion

        #region IsReset

        public bool IsReset
        {
            get { return (bool)GetValue(IsResetProperty); }
            set { SetValue(IsResetProperty, value); }
        }

        public static readonly DependencyProperty IsResetProperty =
            DependencyProperty.Register("IsReset", typeof(bool),
                typeof(EmojiPickerToolBar), new PropertyMetadata(false,OnIsResetChanged));

        private static void OnIsResetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolBar = d as EmojiPickerToolBar;
            if (toolBar == null) return;

            if (toolBar.IsReset)
                if(toolBar._searchTextBox != null) 
                    toolBar._searchTextBox.Text = "Search Emojis";
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _searchTextBox = GetTemplateChild<BaseUI.Controls.TextBox.TextBox>("PART_SearchBox");
        }

        private void OnRandomClick()
        {
            Emoji.Data.Emoji emoji = EmojiData.GetRandomEmoji();
            SetValue(EmojiPickerToolArgsProperty, 
                new EmojiPickerToolArgs() { Type = EmojiPickerToolBarType.Random, Data = "Random"+emoji.Text });
        }

        private void OnDeleteClick()
        {
            SetValue(EmojiPickerToolArgsProperty,
                new EmojiPickerToolArgs() { Type = EmojiPickerToolBarType.Delete, Data = "Delete" });
        }

        private void OnSearch(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            SetValue(EmojiPickerToolArgsProperty, new EmojiPickerToolArgs() { Type = EmojiPickerToolBarType.Search, Data = text });
        }

        #region GetTemplateChild
        private T GetTemplateChild<T>(string childName) where T : class
        {
            T child = GetTemplateChild(childName) as T;
            if (child == null)
            {
                throw new Exception($"Error: {childName} is missing from the template or is not a {typeof(T).Name}. A {typeof(T).Name} is required.");
            }
            return child;
        }
        #endregion
    }


    public class EmojiPickerToolArgs
    { 
        public EmojiPickerToolBarType Type { get; set; }
        public string Data { get; set; }
    }
    public enum EmojiPickerToolBarType
    {
        Delete,
        Random,
        Search,
    }
}
