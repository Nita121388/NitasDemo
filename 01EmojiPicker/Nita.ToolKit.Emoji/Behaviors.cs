using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Nita.ToolKit.Emoji
{
    public static class Behaviors
    {
        public static readonly DependencyProperty EmojiRenderingProperty =
            DependencyProperty.RegisterAttached("EmojiRendering", typeof(bool), typeof(Behaviors),
                                                new UIPropertyMetadata(false, EmojiRenderingChanged));

        public static bool GetEmojiRendering(DependencyObject o)
            => (bool)o.GetValue(EmojiRenderingProperty);

        public static void SetEmojiRendering(DependencyObject o, bool value)
            => o.SetValue(EmojiRenderingProperty, value);

        private static void EmojiRenderingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowDocument doc && (bool)e.NewValue)
                doc.Loaded += FlowDocument_Loaded;
        }

        private static void FlowDocument_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FlowDocument doc)
                doc.SubstituteGlyphs();
        }
    }
}
