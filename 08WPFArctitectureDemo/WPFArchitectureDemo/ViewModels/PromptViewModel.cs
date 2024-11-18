using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using WPFArchitectureDemo.Business.DTO;
using WPFArchitectureDemo.Service;
using WPFArchitectureDemo.Service.IService;
using WPFArchitectureDemo.UI.Views;

namespace WPFArchitectureDemo.UI.ViewModels
{
    public partial class PromptViewModel :ObservableObject
    {
        private IPromptService _promptService;
        private PromptsViewModel _promptsViewModel;
        private IMapper _mapper;
        public PromptViewModel() { }

        #region Properties
        #region ID
        private long _iD;
        public long ID
        {
            get { return _iD; }
            set { SetProperty(ref _iD, value); }
        }
        #endregion

        #region Title
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region IsDelete
        private bool _isDelete;
        public bool IsDelete
        {
            get { return _isDelete; }
            set 
            { 
                SetProperty(ref _isDelete, value); 
                UpdateVisibility();
            }
        }
        #endregion

        # region Content
        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }
        #endregion

        #region CreateDateTime
        public DateTime _createDateTime;
        public DateTime CreateDateTime
        {
            get { return _createDateTime; }
            set { SetProperty(ref _createDateTime, value); }
        }
        #endregion

        #region UpdateDateTime
        public DateTime _updateDateTime;
        public DateTime UpdateDateTime
        {
            get { return _updateDateTime; }
            set { SetProperty(ref _updateDateTime, value); }
        }
        #endregion

        #region Usages
        private ObservableCollection<PromptUsageViewModel> _usages;
        public ObservableCollection<PromptUsageViewModel> Usages
        {
            get { return _usages; }
            set { 
                SetProperty(ref _usages, value); 
                UsageCount = value.Count;
            }
        }
        #endregion

        #region Visibility

        [ObservableProperty]
        public Visibility _visibility;
        
        #endregion

        [ObservableProperty]
        public int _usageCount;
        #endregion

        #region Command
        [RelayCommand]
        public void Delete()
        {
            var result = _promptService.Delete(ID);
            if (result.State == State.Success)
            {
                _promptsViewModel.Delete(this);
            }
        }

        [RelayCommand]
        public void BeginEdit()
        {
            EditPromptDialog editPromptDialog = new EditPromptDialog(this);
            editPromptDialog.ShowDialog();
        }

        [RelayCommand]
        public void Update() 
        { 
            var dto = _mapper.Map<PromptDTO>(this);
            var result = _promptService.Update(dto);
            if (result.State == State.Success)
            {
                _promptsViewModel.Update(this);
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
        }

        [RelayCommand]
        public void Use()
        {
            var result = _promptService.Use(ID);
            if (result.State == State.Success)
            {
                Clipboard.SetText(result.Data.Content);
                UsageCount++;
                MessageBox.Show("使用成功");
            }
            else
            {
                MessageBox.Show("使用失败");
            }
        }
        #endregion

        private void UpdateVisibility()
        {
            Visibility = IsDelete? Visibility.Collapsed : Visibility.Visible;
        }

        public void Init(IPromptService promptService, IMapper mapper, PromptsViewModel promptsViewModel)
        {
            _promptService = promptService;
            _mapper = mapper;
            _promptsViewModel = promptsViewModel;
            UsageCount = Usages.Count;
        }
    }
}
