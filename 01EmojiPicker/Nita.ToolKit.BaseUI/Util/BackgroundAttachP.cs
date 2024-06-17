using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Util
{
    public class BackgroundAttachP
    {
        #region ExtOpacity

        public static readonly DependencyProperty ExtOpacityProperty =
        DependencyProperty.RegisterAttached(
            "ExtOpacity",
            typeof(double),
            typeof(BackgroundAttachP),
            new PropertyMetadata(1.0, OnExtOpacityChanged));
        public static void SetExtOpacity(DependencyObject obj, double value)
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Alpha value must be between 0 and 1.");
            }
            obj.SetValue(ExtOpacityProperty, value);
        }

        public static double GetExtOpacity(DependencyObject obj)
        {
            return (double)obj.GetValue(ExtOpacityProperty);
        }

        private static void OnExtOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetControlOpacity(d);
        }

        #endregion

        #region ExtBrush

        public static readonly DependencyProperty ExtBrushProperty =
        DependencyProperty.RegisterAttached(
            "ExtBrush",
            typeof(Brush),
            typeof(BackgroundAttachP),
            new PropertyMetadata(null, OnExtBrushChanged));

        public static void SetExtBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(ExtBrushProperty, value);
        }

        public static Brush GetExtBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ExtBrushProperty);
        }

        private static void OnExtBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetControlOpacity(d);
        }
        #endregion

        private static void SetControlOpacity(DependencyObject d)
        {
            var brush = GetExtBrush(d);
            var opacity = GetExtOpacity(d);

            if (opacity >= 0 && opacity <= 1 && brush is SolidColorBrush solidColorBrush)
            {
                var color = solidColorBrush.Color;
                color.A = (byte)(int)(255 * opacity);

                if (d is Control)
                {
                    d.SetValue(Control.BackgroundProperty, new SolidColorBrush(color));
                }
                else if (d is Panel)
                {
                    d.SetValue(Panel.BackgroundProperty, new SolidColorBrush(color));
                }
                else if (d is Border)
                {
                    d.SetValue(Border.BackgroundProperty, new SolidColorBrush(color));
                }
            }
        }
    }

}
