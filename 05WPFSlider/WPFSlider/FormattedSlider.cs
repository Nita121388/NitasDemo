using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
namespace WPFSlider
{

    public class FormattedSlider : System.Windows.Controls.Slider
    {
        private ToolTip autoToolTip;
        private string _autoToolTipFormat;
        private int _decimalPlaces;

        /// <summary>
        /// Gets or sets the format string for the auto tooltip.
        /// </summary>
        public string AutoToolTipFormat
        {
            get { return _autoToolTipFormat; }
            set { _autoToolTipFormat = value; }
        }
        /// <summary>
        /// Gets or sets the number of decimal places to display.
        /// </summary>
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set { _decimalPlaces = value; }
        }

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            this.FormatAutoToolTipContent();
        }

        protected override void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            base.OnThumbDragDelta(e);
            this.FormatAutoToolTipContent();
        }

        private void FormatAutoToolTipContent()
        {
            if (!string.IsNullOrEmpty(this.AutoToolTipFormat))
            {
                string formatString = "{0:F" + this.DecimalPlaces + "}";
                this.AutoToolTip.Content = string.Format(this.AutoToolTipFormat, string.Format(formatString, this.Value));
            }
        }

        private ToolTip AutoToolTip
        {
            get
            {
                if (autoToolTip == null)
                {
                    FieldInfo field = typeof(System.Windows.Controls.Slider).GetField("_autoToolTip", BindingFlags.NonPublic | BindingFlags.Instance);
                    autoToolTip = field.GetValue(this) as ToolTip;
                }
                return autoToolTip;
            }
        }
    }
}
