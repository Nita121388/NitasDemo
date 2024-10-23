using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DragBorder
{
    public partial class DragBorderViewModel : ObservableObject
    {
        private DragBorderControl _source;
        private TranslateTransform _translateTransform;
        public Point mouseDownPosition;
        public Point initialTransformPosition; 
        public DragBorderViewModel(DragBorderControl uiElement)
        {
            _source = uiElement;
            DragEvent += Drag;
        }

        #region Properties

        #region DragData
        private object _dragData;
        public object DragData
        {
            get => _dragData;
            set
            {
                _source.DragData = value;
                SetProperty(ref _dragData, value);
            }
        }
        #endregion

        #region IsDragSource
        private bool _isDragSource;
        public bool IsDragSource
        {
            get => _isDragSource;
            set => SetProperty(ref _isDragSource, value);
        }
        #endregion

        #region Source

        #region IsDragging
        private bool _isDragging;
        public bool IsDragging
        {
            get => _isDragging;
            set => SetProperty(ref _isDragging, value);
        }
        #endregion

        #region ParentControl
        private FrameworkElement _parentControl;
        public FrameworkElement ParentControl
        {
            get => _parentControl;
            set
            {
                if (value != null && value!= _parentControl)
                {
                    SetProperty(ref _parentControl, value);
                }
            }
        }
        #endregion

        #region IsRemoveAfterMerage
        private bool _isRemoveAfterMerage;
        public bool IsRemoveAfterMerage
        {
            get => _isRemoveAfterMerage;
            set => SetProperty(ref _isRemoveAfterMerage, value);
        }
        #endregion

        #region TargetControl
        private DragBorderControl _targetControl;
        public DragBorderControl TargetControl
        {
            get => _targetControl;
            set
            {
                SetProperty(ref _targetControl, value);
            }
        }
        #endregion

        #endregion

        #region Target

        #region IsDragEnter
        private bool _isDragEnter;
        public bool IsDragEnter
        {
            get => _isDragEnter;
            set => SetProperty(ref _isDragEnter, value);
        }
        #endregion

        #endregion

        #endregion

        public event EventHandler<DragEventArgs> DragEvent;
        public event EventHandler<DragEventArgs> DragEnterEvent;
        public event EventHandler<DragEventArgs> DragLeaveEvent;

        #region Commands

        #region Source Command
        [RelayCommand]
        private void DragStart(MouseEventArgs e)
        {
            IsDragSource = true;
            mouseDownPosition = e.GetPosition(this.ParentControl);
            if (_translateTransform != null)
            {
                initialTransformPosition = new Point(_translateTransform.X, _translateTransform.Y);
            }
            else
            {
                _translateTransform = _source.PART_Border?.RenderTransform as TranslateTransform;
                initialTransformPosition = new Point(0, 0); // 如果没有Transform，则初始位置为(0,0)  
            }
            _source.CaptureMouse();
        }

        [RelayCommand]
        public void DragMove(MouseEventArgs e)
        {
            if (_source.IsMouseCaptured)
            {
                IsDragging = true;
                if (_translateTransform == null)
                {
                    ReleaseMouseCapture(_source);
                    return;
                }

                //计算偏移量,移动控件
                Point currentPoint = Mouse.GetPosition(this.ParentControl);
                double offsetX = currentPoint.X - mouseDownPosition.X;
                double offsetY = currentPoint.Y - mouseDownPosition.Y;
                double newX = initialTransformPosition.X + offsetX;
                double newY = initialTransformPosition.Y + offsetY;

                if (newX == 0 && newY == 0) return;

                _translateTransform.X = newX;
                _translateTransform.Y = newY;

                //是否进入目标区域
                var hitTarget = HitTestHelper.GetHitDropControls(this.ParentControl, currentPoint);
                if (hitTarget != null && hitTarget != TargetControl)
                {
                    //如果目标区域发生变化，则触发DragLeave事件
                    if (TargetControl != null && TargetControl.ViewModel.IsDragEnter)
                    {
                        TargetControl.ViewModel.IsDragEnter = false;
                        DragLeaveEvent?.Invoke(this, new DragEventArgs(DragData, _source, TargetControl));
                    }
                    //更新目标区域，触发DragEnter事件
                    TargetControl = hitTarget;
                    hitTarget.ViewModel.IsDragEnter = true;
                    DragEnterEvent?.Invoke(this, new DragEventArgs(DragData,_source, TargetControl));
                }
                else if (hitTarget == null)
                {
                    //如果没有进入目标区域，原目标区域触发DragLeave事件
                    if (TargetControl != null)
                    {
                        TargetControl.ViewModel.IsDragEnter = false;
                        TargetControl.ViewModel.DragLeaveEvent?.Invoke(this, new DragEventArgs(DragData, _source, TargetControl));
                        TargetControl = null;
                    }
                }

            }
        }

        [RelayCommand]
        public void Drag(MouseButtonEventArgs e)
        {
            if (TargetControl != null)
            {
                TargetControl.ViewModel.DragEvent?.Invoke(this, new DragEventArgs(DragData, _source, TargetControl));
            }

            if (_source.IsMouseCaptured)
            {
                ReleaseMouseCapture(_source);
            }
        }

        #region Event
        private void Drag(object? sender, DragEventArgs e)
        {
            if (e.Target != null && this._source ==  e.Target && e.Target.ViewModel.IsDragEnter)
            {
                this.DragData = MerageDragData(e);
                if (e.Source.ViewModel.IsRemoveAfterMerage)
                {
                    RemoveAfterMerage(e.Source);
                }
                e.Target.ViewModel.IsDragEnter = false;
            }
        }
        #endregion


        #endregion

        #endregion


        #region Private Methods
        private void ReleaseMouseCapture(UIElement element = null)
        {
            IsDragging = false;
            IsDragSource = false;
            IsDragEnter = false;
            if (TargetControl != null && TargetControl.ViewModel.IsDragEnter)
            { 
                IsDragEnter = false;
            }
            TargetControl = null;
            if (element != null)
                element.ReleaseMouseCapture();
        }
        private IEnumerable<DataItem> MerageDragData(DragEventArgs e)
        {
            var thisDragDataList = EnsureList(this.DragData).Cast<DataItem>().ToList();
            var sourceDragDataList = EnsureList(e.DragData).Cast<DataItem>().ToList();

            thisDragDataList.AddRange(sourceDragDataList);

            return thisDragDataList;
        }

        public List<DataItem> EnsureList(object dragData)
        {
            if (dragData is List<DataItem> existingList)
            {
                return existingList;
            }
            else if (dragData is DataItem singleItem)
            {
                return new List<DataItem> { singleItem };
            }
            else if (dragData is String singleString)
            {
                return new List<DataItem> { new DataItem { Value = singleString, Type = "String" } };
            }
            else if (dragData is List<string> singleStringList)
            {
                return singleStringList.Select(s => new DataItem { Value = s, Type = "String" }).ToList();
            }
            else if (dragData is null)
            {
                return new List<DataItem>();
            }
            else
            {
                // 处理其他类型的数据，可能需要转换  
                throw new ArgumentException("Unsupported drag data type");
            }
        }
        private void RemoveAfterMerage(DragBorderControl source)
        {
            if(source.ParentControl == null)
            {
                return;
            }
            if (source.ParentControl is Panel panel)
            {
                panel.Children.Remove(source);
            }
            else if (source.ParentControl is Grid grid)
            {
                grid.Children.Remove(source);
            }
            else if (source.ParentControl is Canvas canvas)
            {
                canvas.Children.Remove(source);
            }
        }

        #endregion
    }

    public class DragEventArgs : EventArgs
    {
        public object DragData { get; set; }
        public DragBorderControl Source { get; set; }
        public DragBorderControl Target { get; set; }

        public DragEventArgs(object dragData, DragBorderControl source, DragBorderControl target)
        {
            DragData = dragData;
            Source = source;
            Target = target;
        }

    }
    public class DataItem
    {
        public object Value { get; set; }
        public string Type { get; set; } // 用来标识数据类型，例如 "String" 或 "Int"  
    }
}
