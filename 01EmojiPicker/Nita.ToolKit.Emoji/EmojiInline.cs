﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using Controls = System.Windows.Controls;
using EmojiData = Nita.ToolKit.Emoji.Data.EmojiData;

namespace Nita.ToolKit.Emoji
{
    public class EmojiInline : InlineUIContainer
    {
        // Need an empty constructor for serialisation (undo/redo)
        public EmojiInline()
            : base(new Controls.Image())
        {
            // FIXME: not sure TextBottom is the correct value; but Baseline does not work.
            BaselineAlignment = BaselineAlignment.TextBottom;
        }

        public EmojiInline(TextPointer pos)
            : base(new Controls.Image(), pos)
        {
            BaselineAlignment = BaselineAlignment.TextBottom;
        }

        /// <summary>
        /// Redeclare the Child property to prevent it from being serialized.
        /// </summary>
        public new Controls.Image Child
        {
            get => base.Child as Controls.Image;
            private set => base.Child = value;
        }

        protected bool ShouldSerializeChild() => false;

        /// <summary>
        /// The Text property may contain an emoji sequence or a colon-delimited name.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(EmojiInline),
            new PropertyMetadata(""));

        protected override bool ShouldSerializeProperty(DependencyProperty dp)
            => dp.Name == nameof(Text) && base.ShouldSerializeProperty(dp);

        private string UnicodeSequence;

        private struct CacheItem
        {
            internal DrawingImage di;
            internal double width, height;
        };

        // FIXME: the cache should not be in this class
        private static readonly Dictionary<string, CacheItem> m_cache = new Dictionary<string, CacheItem>();

        // FIXME: this is not a very nice API
        internal static void InvalidateCache(string sequence)
            => m_cache.Remove(sequence);

        internal static void Refresh()
            => m_cache.Clear();

        private void Rebuild()
        {
            // FIXME: How can Child be null in Sample.exe? Investigate.
            if (Child == null)
                return;

            if (string.IsNullOrEmpty(UnicodeSequence))
            {
                Child.Source = null;
                return;
            }

            if (!m_cache.TryGetValue(UnicodeSequence, out var item))
            {
                var dg = EmojiImage.RenderEmoji(UnicodeSequence, out item.width, out item.height);
                item.di = new DrawingImage(dg);
                item.di.Freeze();

                m_cache[UnicodeSequence] = item;
            }

            // If there is a tint color, apply a shader effect
            if (Foreground is SolidColorBrush scb && scb.Color != Colors.Black)
                Child.Effect = new TintEffect() { Tint = scb.Color };
            else
                Child.Effect = null;

            Child.Source = item.di;
            Child.Width = item.width * FontSize;
            Child.Height = item.height * FontSize;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == TextProperty)
            {
                if (string.IsNullOrEmpty(Text))
                    UnicodeSequence = null;
                else if (EmojiData.LookupByName.TryGetValue(Text.Trim(':'), out var emoji))
                    UnicodeSequence = emoji.Text;
                else if (EmojiData.LookupByCNName.TryGetValue(Text.Trim(':'), out emoji))
                    UnicodeSequence = emoji.Text;
                else
                    UnicodeSequence = Text;
            }

            // FIXME: split this into several code paths
            if (e.Property == FontSizeProperty || e.Property == TextProperty
                 || e.Property == ForegroundProperty)
            {
                Rebuild();
            }
        }
    }
}
