using CommunityToolkit.Mvvm.Input;
using Nita.ToolKit.Emoji.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Nita.ToolKit.EmojiUI.Controls.EmojiPicker
{

    [TemplatePart(Name = "PART_EmojiPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_SearchBox", Type = typeof(BaseUI.Controls.TextBox.TextBox))]
    public class EmojiPopup : ContentControl
    {
        private Popup _EmojiPopup;
        private BaseUI.Controls.TextBox.TextBox _searchTextBox;
        private EmojiVariation _VPopup;
        private ToggleButton _SelectedEmoji;

        #region Contructors
        static EmojiPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiPopup),
                new FrameworkPropertyMetadata(typeof(EmojiPopup)));
        }

        public EmojiPopup()
        {
            SetValue(EmojiPickedCommandProperty, new RelayCommand<ToggleButton>(OnEmojiPicked)); //todo 也修改为PreviewMouseDown时触发
        }
        #endregion

        #region OnApplyTemplate
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _EmojiPopup = GetTemplateChild<Popup>("PART_EmojiPopup");
            _EmojiPopup.Loaded += OnPopupLoaded;
            _EmojiPopup.KeyDown += OnPopupKeyDown;
        }

        #endregion

        #region Property

        #region EmojiGroups
        public IList<Group> EmojiGroups => EmojiData.AllGroups;

        #endregion

        #region Selection
        public string Selection
        {
            get => (string)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
         nameof(Selection), typeof(string), typeof(EmojiPopup),
             new FrameworkPropertyMetadata("", OnSelectionPropertyChanged));

        private static void OnSelectionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EmojiPopup)?.OnSelectionChanged(e.NewValue as string);
        }

        private void OnSelectionChanged(string s)
        {
            if(!string.IsNullOrEmpty(s)) AddHistorySearchEmojiList(s);
            SelectionChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selection)));
        }
        #endregion

        #region IsOpen 
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(EmojiPopup),
                new PropertyMetadata(false, OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmojiPopup popup)
            {
                popup.SetValue(SearchEmojiListProperty, new List<IEnumerable<Emoji.Data.Emoji>>());
                /*if (!popup.IsOpen)
                {
                    popup.SetValue(SearchEmojiListProperty, new List<IEnumerable<Emoji.Data.Emoji>>());
                }*/
            }
        }

        #endregion

        #region SearchEmojiList
        public IEnumerable<IEnumerable<Emoji.Data.Emoji>> SearchEmojiList
        {
            get { return (IEnumerable<IEnumerable<Emoji.Data.Emoji>>)GetValue(SearchEmojiListProperty); }
            set { SetValue(SearchEmojiListProperty, value); }
        }

        public static readonly DependencyProperty SearchEmojiListProperty =
            DependencyProperty.Register("SearchEmojiList", typeof(IEnumerable<IEnumerable<Emoji.Data.Emoji>>), typeof(EmojiPopup),
                new PropertyMetadata(new List<IEnumerable<Emoji.Data.Emoji>>(), OnSearchEmojiListChanged));

        private static void OnSearchEmojiListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmojiPopup popup)
            {
                if (popup.SearchEmojiList != null && popup.SearchEmojiList.Count() > 0)
                {
                    popup.IsShowSearch = true;
                }
                else 
                {
                    popup.IsShowSearch = false;
                }
            }
        }

        public bool IsShowSearch
        {
            get { return (bool)GetValue(IsShowSearchProperty); }
            set { SetValue(IsShowSearchProperty, value); }
        }

        public static readonly DependencyProperty IsShowSearchProperty =
            DependencyProperty.Register("IsShowSearch", typeof(bool), typeof(EmojiPopup), new PropertyMetadata(false));


        #endregion

        #region HistorySelectionEmojiList
        public IEnumerable<IEnumerable<Emoji.Data.Emoji>> HistorySelectionEmojiList
        {
            get { return (IEnumerable<IEnumerable<Emoji.Data.Emoji>>)GetValue(HistorySelectionEmojiListProperty); }
            set { SetValue(HistorySelectionEmojiListProperty, value); }
        }

        public static readonly DependencyProperty HistorySelectionEmojiListProperty =
            DependencyProperty.Register("HistorySelectionEmojiList", typeof(IEnumerable<IEnumerable<Emoji.Data.Emoji>>), typeof(EmojiPopup),
                new PropertyMetadata(new List<IEnumerable<Emoji.Data.Emoji>>(), OnHistorySelectionEmojiListChanged));

        private static void OnHistorySelectionEmojiListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmojiPopup popup)
            {
                if (popup.HistorySelectionEmojiList != null && popup.HistorySelectionEmojiList.Count() > 0)
                {
                    popup.IsShowHistorySelection = true;
                }
                else
                {
                    popup.IsShowHistorySelection = false;
                }
            }
        }
        public bool IsShowHistorySelection
        {
            get { return (bool)GetValue(IsShowHistorySelectionProperty); }
            set { SetValue(IsShowHistorySelectionProperty, value); }
        }

        public static readonly DependencyProperty IsShowHistorySelectionProperty =
            DependencyProperty.Register("IsShowHistorySelection", typeof(bool), typeof(EmojiPopup), new PropertyMetadata(false));

        public void AddHistorySearchEmojiList(string selection)
        {
            var HistorySearchEmojiStrs = Settings.Default.HistorySearchEmoji.Split(",")
                .Where(r => !string.IsNullOrEmpty(r))
                .ToHashSet();

            HistorySearchEmojiStrs.Add(selection);

            if (HistorySearchEmojiStrs.Count > 15)
            {
                HistorySearchEmojiStrs.Remove(HistorySearchEmojiStrs.Last());
            }

            Settings.Default.HistorySearchEmoji = string.Join(",", HistorySearchEmojiStrs);
            Settings.Default.Save();
            InitHistorySearchEmojiList();
        }

        public void InitHistorySearchEmojiList()
        {
            var list = EmojiData.GetEmojisByText(Settings.Default.HistorySearchEmoji.Split(",").ToList());
            SetValue(HistorySelectionEmojiListProperty, list);
        }
        #endregion

        #region EmojiPickerToolArgs

        public EmojiPickerToolArgs EmojiPickerToolArgs
        {
            get { return (EmojiPickerToolArgs)GetValue(EmojiPickerToolArgsProperty); }
            set { SetValue(EmojiPickerToolArgsProperty, value); }
        }

        public static readonly DependencyProperty EmojiPickerToolArgsProperty =
            DependencyProperty.Register("EmojiPickerToolArgs", typeof(EmojiPickerToolArgs), typeof(EmojiPopup), new PropertyMetadata(OnEmojiPickerToolArgsChanged));

        private static void OnEmojiPickerToolArgsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EmojiPopup popup)
            {
                if (popup.EmojiPickerToolArgs is not null)
                {
                    if (popup.EmojiPickerToolArgs.Type == EmojiPickerToolBarType.Random)
                    {
                        popup.SetValue(SelectionProperty, popup.EmojiPickerToolArgs.Data);
                    }
                    if (popup.EmojiPickerToolArgs.Type == EmojiPickerToolBarType.Delete)
                    {
                        popup.SetValue(SelectionProperty, popup.EmojiPickerToolArgs.Data);
                    }
                    if (popup.EmojiPickerToolArgs.Type == EmojiPickerToolBarType.Search)
                    {
                        var emojis = EmojiData.GetValue(popup.EmojiPickerToolArgs.Data);
                        popup.SetValue(SearchEmojiListProperty, emojis);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Support  Events
        public event PropertyChangedEventHandler SelectionChanged;
        //public event PropertyChangedEventHandler Opened;

        #region Static Event Register

        #region EmojiPickedCommand
        public IRelayCommand EmojiPickedCommand => (IRelayCommand)GetValue(EmojiPickedCommandProperty);

        public static readonly DependencyProperty EmojiPickedCommandProperty = DependencyProperty.Register(
            nameof(EmojiPickedCommand),
            typeof(IRelayCommand),
            typeof(EmojiPopup),
            new PropertyMetadata(null)
        );

        public static readonly RoutedEvent PickedEvent = EventManager.RegisterRoutedEvent(
            nameof(Picked),
            RoutingStrategy.Bubble,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPopup)
        );

        /// <summary>
        ///  选择Emoji
        /// </summary>
        public event EmojiPickedEventHandler Picked
        {
            add => AddHandler(PickedEvent, value);
            remove => RemoveHandler(PickedEvent, value);
        }
        #endregion

        #region RandomedCommand
        public IRelayCommand RandomedCommand => (IRelayCommand)GetValue(RandomedCommandProperty);

        public static readonly DependencyProperty RandomedCommandProperty = DependencyProperty.Register(
            nameof(RandomedCommand),
            typeof(IRelayCommand),
            typeof(EmojiPopup),
            new PropertyMetadata(null)
        );

        public static readonly RoutedEvent RandomedEvent = EventManager.RegisterRoutedEvent(
            nameof(Randomed),
            RoutingStrategy.Direct,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPopup)
        );

        /// <summary>
        ///  Random Emoji
        /// </summary>
        public event EmojiPickedEventHandler Randomed
        {
            add => AddHandler(PickedEvent, value);
            remove => RemoveHandler(PickedEvent, value);
        }
        #endregion

        #region DeletedCommand
        public IRelayCommand DeletedCommand => (IRelayCommand)GetValue(DeletedCommandProperty);

        public static readonly DependencyProperty DeletedCommandProperty = DependencyProperty.Register(
            nameof(DeletedCommand),
            typeof(IRelayCommand),
            typeof(EmojiPopup),
            new PropertyMetadata(null)
        );

        public static readonly RoutedEvent DeletedEvent = EventManager.RegisterRoutedEvent(
            nameof(Deleted),
            RoutingStrategy.Bubble,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPopup)
        );

        /// <summary>
        ///  Random Emoji
        /// </summary>
        public event EmojiPickedEventHandler Deleted
        {
            add => AddHandler(PickedEvent, value);
            remove => RemoveHandler(PickedEvent, value);
        }
        #endregion

        #region SearchBoxPreviewMouseLeftButtonDownCommand
        public IRelayCommand SearchCommand => (IRelayCommand)GetValue(SearchCommandProperty);

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            nameof(SearchCommand),
            typeof(IRelayCommand),
            typeof(EmojiPopup),
            new PropertyMetadata(null)
        );

        #endregion


        #endregion

        #region ClosedEvent
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
            nameof(Closed),
            RoutingStrategy.Bubble,
            typeof(EmojiPickedEventHandler),
            typeof(EmojiPopup)
        );

        /// <summary>
        ///  关闭Emoji弹窗
        /// </summary>
        public event EmojiPickedEventHandler Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
        #endregion

        #endregion

        #region Events

        private void OnPopupLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not Popup popup)
                return;

            popup.Closed += popupClosed;
            popup.LostFocus += Popup_LostFocus;

        }

        private void OnEmojiPicked(ToggleButton toggle)
        {
            if (toggle == null) return;
            if (toggle.IsChecked == false) toggle.IsChecked = true;
            if (toggle.DataContext is Emoji.Data.Emoji emoji)
            {
                _SelectedEmoji = toggle;
                if (emoji.VariationList.Count == 0)
                {
                    Selection = emoji.Text;
                    toggle.IsChecked = false;
                    RaiseEvent(new EmojiPickedEventArgs(PickedEvent, toggle, Selection)); //暂时没有订阅
                    SetValue(IsOpenProperty, false);
                }
                else
                {
                    _VPopup = FindVisualChild<EmojiVariation>(toggle);
                    _VPopup.IsOpen = true;
                    _VPopup.IsOpenChanged += VPopup_IsOpenChanged;
                }
            }
        }

        #region VPopup Event

        private void VPopup_IsOpenChanged(object sender, IsOpenChangedEventArgs e)
        {
            if (sender is EmojiVariation popup && popup.IsOpen == false)
            {
                _SelectedEmoji.IsChecked = false;
                SetValue(IsOpenProperty, false);
                _VPopup.IsOpenChanged -= VPopup_IsOpenChanged;
            }
        }
        #endregion

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source is Popup
                || e.Source is not BaseUI.Controls.TextBox.TextBox
                || e.Source is not Button
                || e.Source is not ListView)
            {
                e.Handled = true;
                return;
            }
            SetValue(IsOpenProperty, false);
        }

        private void popupClosed(object? sender, EventArgs e)
        {
            Keyboard.Focus(Keyboard.FocusedElement);
            SetValue(IsOpenProperty, false);
        }

        private void OnPopupKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && sender is Popup popup)
            {
                SetValue(IsOpenProperty, false);
                e.Handled = true;
            }
        }

        #endregion

        #region override

        #region GetTemplateChild
        /// <summary>
        /// 获取模板中的子元素。
        /// </summary>
        /// <typeparam name="T">子元素的类型。</typeparam>
        /// <param name="childName">子元素的名称。</param>
        /// <returns>返回指定类型的子元素。</returns>
        public T GetTemplateChild<T>(string childName) where T : class
        {
            T child = GetTemplateChild(childName) as T;
            if (child == null)
            {
                throw new Exception($"Error: {childName} is missing from the template or is not a {typeof(T).Name}. A {typeof(T).Name} is required.");
            }
            return child;
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #endregion

        #endregion

    }
}
