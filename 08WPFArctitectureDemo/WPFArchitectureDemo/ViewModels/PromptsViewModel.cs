using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using WPFArchitectureDemo.Service.IService;
using WPFArchitectureDemo.Service;
using WPFArchitectureDemo.UI.Common.Helper;
using System.Windows;
using System.Windows.Input;
using WPFArchitectureDemo.Business.DTO;

namespace WPFArchitectureDemo.UI.ViewModels
{
    public partial class PromptsViewModel : ObservableObject
    {
        private IPromptService _promptService;
        private IMapper _mapper;
        public PromptsViewModel()
        {
            _promptService = App.Current.Services.GetRequiredService<IPromptService>();
            _mapper = App.Current.Services.GetRequiredService<IMapper>();
        }

        #region Properties

        #region ItemsLists
        private ObservableCollection<ObservableCollection<PromptViewModel>> _itemsLists = new ObservableCollection<ObservableCollection<PromptViewModel>>();
        public ObservableCollection<ObservableCollection<PromptViewModel>> ItemsLists
        {
            get { return _itemsLists; }
            set { SetProperty(ref _itemsLists, value); }
        }
        #endregion

        #region NewPromptTitle
        private string _newPromptTitle = "标题";

        public string NewPromptTitle
        {
            get { return _newPromptTitle; }
            set { SetProperty(ref _newPromptTitle, value); }
        }
        #endregion

        #region NewPromptContent
        private string _newPromptContent = "内容";

        public string NewPromptContent
        {
            get { return _newPromptContent; }
            set { SetProperty(ref _newPromptContent, value); }
        }

        #endregion

        #region keywords
        private string _keywords = "关键字";

        public string Keywords
        {
            get { return _keywords; }
            set { SetProperty(ref _keywords, value); }
        }
        #endregion

        #region ContentVisibility
        private Visibility _contentVisibility = Visibility.Collapsed;
        public Visibility ContentVisibility
        {
            get { return _contentVisibility; }
            set { SetProperty(ref _contentVisibility, value); }
        }
        #endregion

        #region ColumnCount
        private int _columnCount = 2;
        public int ColumnCount
        {
            get { return _columnCount; }
            set { SetProperty(ref _columnCount, value); }
        }
        #endregion

        #endregion

        #region Command
        [RelayCommand]
        public void Refresh()
        {
            var result = _promptService.Select("IsDelete = 0", "UpdateDateTime desc");
            if (result.State == State.Success)
            {
                ItemsLists.Clear();
                result.Data.OrderBy(d => d.Usages.Count)
                    .ToList()
                    .ForEach(dto => AddItemsLists(dto));
            }
        }

        [RelayCommand]
        public void Add()
        {
            var result = _promptService.Create(NewPromptTitle, NewPromptContent);
            if (result.State == State.Success)
            {
                var dto = result.Data;
                AddItemsLists(dto);
            }
            NewPromptTitle = "标题";
            NewPromptContent = "内容";
            ContentVisibility = Visibility.Collapsed;
        }

        [RelayCommand]
        public void Import()
        {
            var filePath = FileHelper.SelectFile();
            _promptService.Import(filePath);
            Refresh();
            MessageBox.Show("导入成功!");
        }

        [RelayCommand]
        public void Export()
        {
            var folderPath = FileHelper.SelectFolder();
            _promptService.Export(folderPath);
            MessageBox.Show($"导出成功，路径：{folderPath}");
        }

        [RelayCommand]
        public void UpdateTitle()
        {
            ContentVisibility = Visibility.Visible;
        }

        [RelayCommand]
        public void KeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var result = _promptService.FuzzySelect(Keywords);
                if (result.State == State.Success)
                {
                    ItemsLists.Clear();
                    foreach (var dto in result.Data)
                    {
                        AddItemsLists(dto);
                    }
                }
                Keywords = "关键词";
            }
        }
        #endregion

        public void Delete(PromptViewModel prompt)
        {
            var itemsList = ItemsLists.FirstOrDefault(x => x.Contains(prompt));
            if (itemsList != null)
            {
                itemsList.Remove(prompt);
            }

        }

        public void Update(PromptViewModel prompt)
        {
            var itemsList = ItemsLists.FirstOrDefault(x => x.Contains(prompt));
            if (itemsList!= null)
            {
                var index = itemsList.IndexOf(prompt);
                itemsList[index] = prompt;
            }
        }

        private void AddItemsLists(PromptDTO dto)
        {
            var promptViewModel = _mapper.Map<PromptViewModel>(dto);
            promptViewModel.Init(_promptService, _mapper, this);

            var lastRow = ItemsLists.LastOrDefault();
            if (lastRow == null)
            {
                lastRow = new ObservableCollection<PromptViewModel>();
                ItemsLists.Add(lastRow);
            }

            if (lastRow.Count < ColumnCount)
            {
                lastRow.Add(promptViewModel);
            }
            else
            {
                var newRow = new ObservableCollection<PromptViewModel>();
                newRow.Add(promptViewModel);
                ItemsLists.Add(newRow);
            }
        }
    }
}
