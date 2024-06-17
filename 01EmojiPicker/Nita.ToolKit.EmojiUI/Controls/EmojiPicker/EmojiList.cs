using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nita.ToolKit.Emoji.Data;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SysPrim = System.Windows.Controls.Primitives;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Windows.Media;

namespace Nita.ToolKit.EmojiUI.Controls.EmojiPicker
{
    public class EmojiList : ContentControl
    {
        private ToggleButton _SelectedEmoji;
        private EmojiVariation _VPopup;

        #region Contructors
        static EmojiList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiList),
                new FrameworkPropertyMetadata(typeof(EmojiList)));
        }
        public EmojiList()
        {
            SetValue(EmojiPickedCommandProperty, new RelayCommand<ToggleButton>(OnEmojiPicked));
        }
        #endregion

        #region Data

        #region EmojiChunkList

        public IEnumerable<IEnumerable<Emoji.Data.Emoji>> EmojiChunkList
        {
            get { return (IEnumerable<IEnumerable<Emoji.Data.Emoji>>)GetValue(EmojiChunkListProperty); }
            set { SetValue(EmojiChunkListProperty, value); }
        }

        public static readonly DependencyProperty EmojiChunkListProperty =
            DependencyProperty.Register("EmojiChunkList",
                typeof(IEnumerable<IEnumerable<Emoji.Data.Emoji>>),
                typeof(EmojiList),
                new PropertyMetadata());

        #endregion

        #endregion

        #region Selection

        public event PropertyChangedEventHandler SelectionChanged;
        public string Selection
        {
            get => (string)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
         nameof(Selection), typeof(string), typeof(EmojiList),
             new FrameworkPropertyMetadata("", OnSelectionPropertyChanged));

        private static void OnSelectionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EmojiList)?.OnSelectionChanged(e.NewValue as string);
        }

        private void OnSelectionChanged(string s)
        {
            var is_disabled = string.IsNullOrEmpty(s);
            SelectionChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selection)));
        }
        #endregion

        #region IsOpen

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
         nameof(IsOpen), typeof(bool), typeof(EmojiList),
             new FrameworkPropertyMetadata());

        #endregion

        #region Command

        public IRelayCommand EmojiPickedCommand => (IRelayCommand)GetValue(EmojiPickedCommandProperty);

        public static readonly DependencyProperty EmojiPickedCommandProperty = DependencyProperty.Register(
            nameof(EmojiPickedCommand),
            typeof(IRelayCommand),
            typeof(EmojiList),
            new PropertyMetadata(null)
        );
        #endregion

        #region Event

        #region Picked

        public static readonly RoutedEvent PickedEvent = EventManager.RegisterRoutedEvent(
           nameof(Picked),
           RoutingStrategy.Bubble,
           typeof(EmojiPickedEventHandler),
           typeof(EmojiList)
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

        #endregion

        #region Private Methods

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
                    RaiseEvent(new EmojiPickedEventArgs(PickedEvent, toggle, Selection));
                    IsOpen = false;
                }
                else
                {
                    _VPopup = FindVisualChild<EmojiVariation>(toggle);
                    _VPopup.IsOpen = true;
                    _VPopup.IsOpenChanged += VPopup_IsOpenChanged;
                }
            }
        }

        private void VPopup_IsOpenChanged(object sender, IsOpenChangedEventArgs e)
        {
            if (sender is EmojiVariation popup && popup.IsOpen == false)
            {
                _SelectedEmoji.IsChecked = false;
                RaiseEvent(new EmojiPickedEventArgs(PickedEvent, _SelectedEmoji, Selection));
                IsOpen = false;
                _VPopup.IsOpenChanged -= VPopup_IsOpenChanged;
            }
        }


        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
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
    }
}
