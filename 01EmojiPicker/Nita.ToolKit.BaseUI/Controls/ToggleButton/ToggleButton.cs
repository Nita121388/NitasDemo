using Nita.ToolKit.BaseUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Nita.ToolKit.BaseUI.Controls.ToggleButton
{
    [TemplatePart(Name = "PART_LeftDockPanel", Type = typeof(DockPanel))]
    [TemplatePart(Name = "PART_RightDockPanel", Type = typeof(DockPanel))]
    public class ToggleButton : System.Windows.Controls.Primitives.ToggleButton
    {
        #region Fields
        private DockPanel _leftDockPanel;
        private DockPanel _rightDockPanel;
        #endregion

        #region Constructor

        static ToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToggleButton),
                new FrameworkPropertyMetadata(typeof(ToggleButton)));

        }
        #endregion

        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftDockPanel = GetTemplateChild("PART_LeftDockPanel") as DockPanel;
            _rightDockPanel = GetTemplateChild("PART_RightDockPanel") as DockPanel;
            UpdateLeftRightDockPanelVisibility();
        }
        #endregion

        #region DependencyProperty

        #region SizeType
        public SizeType SizeType
        {
            get { return (SizeType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        public static readonly DependencyProperty SizeTypeProperty =
            DependencyProperty.Register("SizeType",
                typeof(SizeType),
                typeof(ToggleButton),
                new PropertyMetadata(SizeType.M));
        #endregion

        #region IsChangeIcon
        public bool IsShowChangeIcon
        {
            get { return (bool)GetValue(IsShowChangeIconProperty); }
            set { SetValue(IsShowChangeIconProperty, value); }
        }

        public static readonly DependencyProperty IsShowChangeIconProperty =
            DependencyProperty.Register("IsShowChangeIcon",
                typeof(bool),
                typeof(ToggleButton),
                new PropertyMetadata(true, OnIsShowChangeIconChanged));


        private static void OnIsShowChangeIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleButton = d as ToggleButton;
            if (toggleButton == null)
            {
                return;
            }
            toggleButton.UpdateLeftRightDockPanelVisibility();
        }

        private void UpdateLeftRightDockPanelVisibility()
        {
            if (_leftDockPanel != null)
            {
                _leftDockPanel.Visibility = (HorizontalDirection == HorizontalDirection.Left && IsShowChangeIcon) ?
                    Visibility.Visible : Visibility.Collapsed;
            }
            if (_rightDockPanel != null)
            {
                _rightDockPanel.Visibility = (HorizontalDirection == HorizontalDirection.Right && IsShowChangeIcon) ?
                    Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region UnCheckIcon
        public ImageSource UnCheckIcon
        {
            get { return (ImageSource)GetValue(UnCheckIconProperty); }
            set { SetValue(UnCheckIconProperty, value); }
        }

        public static readonly DependencyProperty UnCheckIconProperty =
            DependencyProperty.Register("UnCheckIcon",
                typeof(ImageSource),
                typeof(ToggleButton),
                new PropertyMetadata(
                    (ImageSource)Application.Current.Resources["LeftTriangle"]
                   ));
        #endregion

        #region CheckedIcon
        public ImageSource CheckedIcon
        {
            get { return (ImageSource)GetValue(CheckedIconProperty); }
            set { SetValue(CheckedIconProperty, value); }
        }

        public static readonly DependencyProperty CheckedIconProperty =
            DependencyProperty.Register("CheckedIcon",
                typeof(ImageSource),
                typeof(ToggleButton),
                new PropertyMetadata(
                    (ImageSource)Application.Current.Resources["DownTriangle"]
                    ));
        #endregion

        #region HorizontalDirection
        public HorizontalDirection HorizontalDirection
        {
            get { return (HorizontalDirection)GetValue(HorizontalDirectionProperty); }
            set { SetValue(HorizontalDirectionProperty, value); }
        }

        public static readonly DependencyProperty HorizontalDirectionProperty =
            DependencyProperty.Register("HorizontalDirection",
                typeof(HorizontalDirection),
                typeof(ToggleButton),
                new PropertyMetadata(HorizontalDirection.Right, OnHorizontalDirectionChanged));

        private static void OnHorizontalDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleButton = d as ToggleButton;
            if (toggleButton == null)
            {
                return;
            }
            toggleButton.UpdateIcon();
        }


        private void UpdateIcon()
        {
            UpdateLeftRightDockPanelVisibility();
            if (HorizontalDirection == HorizontalDirection.Left)
            {
                UnCheckIcon = (ImageSource)Application.Current.Resources["RightTriangle"];
            }
            else
            {
                UnCheckIcon = (ImageSource)Application.Current.Resources["LeftTriangle"];
            }
        }



        #endregion


        #region 鼠标悬浮样式 MouseOverStyle
        /// <summary>
        /// 鼠标悬浮样式
        /// </summary>
        public static readonly DependencyProperty MouseOverStyleProperty =
            DependencyProperty.Register("MouseOverStyle",
                typeof(MouseOverStyle),
                typeof(ToggleButton),
                new PropertyMetadata(MouseOverStyle.ZoomAndChangeColor));
        public MouseOverStyle MouseOverStyle
        {
            get { return (MouseOverStyle)GetValue(MouseOverStyleProperty); }
            set { SetValue(MouseOverStyleProperty, value); }
        }
        #endregion

        #endregion

    }
}
