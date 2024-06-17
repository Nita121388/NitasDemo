using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nita.ToolKit.EmojiUI.Controls
{

    public class EmojiPickedEventArgs : RoutedEventArgs
    {
        public string Emoji;
        public EmojiPickedEventArgs() { }
        public EmojiPickedEventArgs(string emoji) => Emoji = emoji;
        public EmojiPickedEventArgs(RoutedEvent routedEvent, object source, string pickedEmoji)
        : base(routedEvent, source)
        {
            Emoji = pickedEmoji;
        }
    }
    public delegate void EmojiPickedEventHandler(object sender, EmojiPickedEventArgs e);

    public class IsOpenChangedEventArgs : RoutedEventArgs
    {
        public bool IsOpen;
        public IsOpenChangedEventArgs() { }
        public IsOpenChangedEventArgs(bool isOpen) => IsOpen = isOpen;
        public IsOpenChangedEventArgs(RoutedEvent routedEvent, object source, bool isOpen)
        : base(routedEvent, source)
        {
            IsOpen = isOpen;
        }
    }

    public delegate void IsOpenChangedEventHandler(object sender, IsOpenChangedEventArgs e);
}
