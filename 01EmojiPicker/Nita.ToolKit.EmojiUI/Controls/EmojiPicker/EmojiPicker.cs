using Nita.ToolKit.BaseUI.Entity;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SysPrim = System.Windows.Controls.Primitives;

namespace Nita.ToolKit.EmojiUI.Controls.EmojiPicker
{
    /// <summary>
    /// Selection:
    ///     1. 与其内部（EmojiPopup）PART_NitaEmojiPopup 的 Selection 双向绑定
    ///     => EmojiPopup的 Selection发生变化时，EmojiPicker发生变化 
    /// IsChecked:
    ///     1. 与其内部（ToggleButton）PART_PickerButton 的 IsChecked 双向绑定
    ///     2. 与其内部（EmojiPopup）PART_NitaEmojiPopup 的 IsOpen 双向绑定
    ///     => PART_PickerButton 触发点击 EmojiPopup打开
    ///        EmojiPopup关闭，PART_PickerButton设为未选择
    /// SizeType
    /// FontSize
    /// ClickColor
    /// </summary>
    [TemplatePart(Name = "PART_PickerButton", Type = typeof(SysPrim.ToggleButton))]
    [TemplatePart(Name = "PART_PickerImage", Type = typeof(Image))]
    [TemplatePart(Name = "PART_NitaEmojiPopup", Type = typeof(EmojiPopup))]
    public class EmojiPicker : ContentControl
    {
        #region Fields
        private Image _PickerImage;
        private EmojiPopup _NitaEmojiPopup;
        #endregion

        #region Contructors
        static EmojiPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmojiPicker),
                new FrameworkPropertyMetadata(typeof(EmojiPicker)));
        }
        #endregion

        #region Property

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType", typeof(SizeType),
                typeof(EmojiPicker),
                new PropertyMetadata(SizeType.XXXL));

        #endregion

        #region FontSize
        public new double FontSize
        {
            get
            {
                if (_PickerImage != null)
                {
                    return _PickerImage.Height * 0.75;
                }
                return 0;
            }
            set
            {
                if (_PickerImage != null)
                {
                    _PickerImage.Height = value / 0.75;
                }
            }
        }
        #endregion

        #region ClickColor
        public Brush ClickColor
        {
            get { return (Brush)GetValue(ClickColorProperty); }
            set { SetValue(ClickColorProperty, value); }
        }

        public static readonly DependencyProperty ClickColorProperty =
            DependencyProperty.Register("ClickColor", typeof(Brush),
                typeof(EmojiPicker),
                new PropertyMetadata());
        #endregion

        #region Selection
        public string Selection
        {
            get => (string)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
         nameof(Selection), typeof(string), typeof(EmojiPicker),
             new FrameworkPropertyMetadata("🍔"));
        #endregion

        #region IsChecked
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
         nameof(IsChecked), typeof(bool), typeof(EmojiPicker),
             new FrameworkPropertyMetadata(false, OnIsCheckedChanged));
        #endregion

        #endregion

        #region override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _PickerImage = GetTemplateChild<Image>("PART_PickerImage");
            _NitaEmojiPopup = GetTemplateChild<EmojiPopup>("PART_NitaEmojiPopup");
            _NitaEmojiPopup.SelectionChanged += NitaEmojiPopup_SelectionChanged;
        }

        private void NitaEmojiPopup_SelectionChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_NitaEmojiPopup.Selection.StartsWith("Random"))
            {
                Selection = _NitaEmojiPopup.Selection.Replace("Random", "");
            }
            else if (_NitaEmojiPopup.Selection.StartsWith("Delete"))
            {
                Selection = _NitaEmojiPopup.Selection.Replace("Delete", "");
            }
            else
            {
                Selection = _NitaEmojiPopup.Selection;
            }
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

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
}
