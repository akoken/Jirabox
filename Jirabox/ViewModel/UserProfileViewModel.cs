using GalaSoft.MvvmLight;
using Jirabox.Core.Contracts;
using Jirabox.Model;

namespace Jirabox.ViewModel
{
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;
        private bool isDataLoaded;
        private bool isTaskbarVisible = true;
        private User user;

        public User User
        {
            get { return user; }
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

        public UserProfileViewModel(IDialogService dialogService, IJiraService jiraService)
        {
            this.dialogService = dialogService;
            this.jiraService = jiraService;            
        }

        public async void Initialize()
        {
            IsDataLoaded = false;            
            User = await jiraService.GetUserProfileAsync(App.UserName);
            IsDataLoaded = true;
        }
    }
}
