using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;

namespace Jirabox.ViewModel
{
    public class AddCommentViewModel : ViewModelBase
    {
        private readonly IJiraService jiraService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        private bool isDataLoaded;
        private User user;
        private string commentText;
        private string issueKey;
        private bool isTaskbarVisible = true;

        public RelayCommand SendCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public string CommentText
        {
            get
            {
                return commentText;
            }
            set
            {
                if (commentText != value)
                {
                    commentText = value;
                    RaisePropertyChanged(() => CommentText);
                }
            }
        }

        public string IssueKey
        {
            get
            {
                return issueKey;
            }
            set
            {
                if (issueKey != value)
                {
                    issueKey = value;
                    RaisePropertyChanged(() => IssueKey);
                }
            }
        }

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user != value)
                {
                    user = value;
                    RaisePropertyChanged(() => User);
                }
            }
        }
        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                if (isDataLoaded != value)
                {
                    isDataLoaded = value;
                    RaisePropertyChanged(() => IsDataLoaded);
                }
            }
        }

        public bool IsTaskbarVisible
        {
            get { return isTaskbarVisible; }
            set
            {
                if (isTaskbarVisible != value)
                {
                    isTaskbarVisible = value;
                    RaisePropertyChanged(() => IsTaskbarVisible);
                }
            }
        }

        public AddCommentViewModel(IJiraService jiraService, IDialogService dialogService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            User = App.User;
            SendCommand = new RelayCommand(SendComment);
            CancelCommand = new RelayCommand(CancelComment);
        }

        private async void SendComment()
        {
            IsDataLoaded = false;
            var success = await jiraService.AddComment(IssueKey, CommentText);
            IsDataLoaded = true;
            IsTaskbarVisible = false;
            if (success)
            {                
                dialogService.ShowDialog(AppResources.CommentAddedMessage, AppResources.Done);
                navigationService.GoBack();
            }
            else
                dialogService.ShowDialog(AppResources.ErrorMessage, AppResources.Error);
            IsTaskbarVisible = true;
        }      
        private void CancelComment()
        {
            navigationService.GoBack();
        }

        public void Initialize(string key)
        {
            IssueKey = key;
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            IssueKey = string.Empty;
            CommentText = string.Empty;
        }
    }
}
