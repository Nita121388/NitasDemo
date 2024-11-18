using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFArchitectureDemo.UI.ViewModels
{
    public partial class PromptUsageViewModel : ObservableObject
    {
        #region ID
        private long _iD;
        public long ID
        {
            get { return _iD; }
            set { SetProperty(ref _iD, value); }
        }
        #endregion

        #region PromptID
        private long _promptID;
        public long PromptID
        {
            get { return _promptID; }
            set { SetProperty(ref _promptID, value); }
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

    }

}
