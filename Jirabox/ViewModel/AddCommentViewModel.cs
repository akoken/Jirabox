using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;

namespace Jirabox.ViewModel
{
    public class AddCommentViewModel : ViewModelBase
    {
        private IJiraService jiraService;
        private IDialogService dialogService;
        private INavigationService navigationService;

        private bool isDataLoaded;
        private User user;
        private string commentText;
        private string issueKey;

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
            if (success)
            {
                dialogService.ShowDialog(AppResources.CommentAddedMessage, AppResources.Done);
                navigationService.GoBack();
            }
            else
                dialogService.ShowDialog(AppResources.ErrorMessage, AppResources.Error);
        }      
        private void CancelComment()
        {
            navigationService.GoBack();
        }

        public void Initialize(string issueKey)
        {
            IssueKey = issueKey;
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            IssueKey = string.Empty;
            CommentText = string.Empty;
        }
    }
}
