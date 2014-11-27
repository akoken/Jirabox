using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Core.ExceptionExtension;
using Jirabox.Resources;
using System;
using System.Threading;

namespace Jirabox.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;

        private CancellationTokenSource cancellationTokenSource = null;
        private string serverUrl;
        private string userName;        
        private string password;
        private bool isDataLoaded;        
        private bool loginButtonEnabled;
        private bool isTaskbarVisible = true;

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }   

        public string ServerUrl
        {
            get { return serverUrl; }
            set 
            {
                if (serverUrl != value)
                {
                    serverUrl = value;
                    App.ServerUrl = serverUrl;
                    RaisePropertyChanged(() => ServerUrl);
                }                
            }
        }

        public bool LoginButtonEnabled
        {
            get
            {
                return loginButtonEnabled;
            }
            set
            {
                loginButtonEnabled = value;
                RaisePropertyChanged(() => LoginButtonEnabled);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get { return userName; }
            set 
            {
                if (userName != value)
                {
                    userName = value;
                    App.UserName = userName;
                    RaisePropertyChanged(() => UserName);
                }
                
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    App.Password = password;
                    RaisePropertyChanged(() => Password);
                }

            }
        }        

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set 
            {
                if (value != isDataLoaded)
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
        
        public LoginViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {            
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;         
   
            InitializeData();
        }

        public async void Login()
        {
            IsDataLoaded = false;
            LoginButtonEnabled = false;
            if (!IsInputsValid())
            {
                IsTaskbarVisible = false;
                IsDataLoaded = true;
                dialogService.ShowDialog(AppResources.UnauthorizedMessage, AppResources.LoginFailedMessage);
                IsTaskbarVisible = true;
                LoginButtonEnabled = true;
                return;
            }

            if (!ValidateUrl(ServerUrl))
            {
                IsDataLoaded = true;
                IsTaskbarVisible = false;
                dialogService.ShowDialog(AppResources.InvalidServerUrlMessage, AppResources.LoginFailedMessage);
                IsTaskbarVisible = true;
                LoginButtonEnabled = true;
                return;
            }
            try
            {
                //Create new cancellation token source
                cancellationTokenSource = new CancellationTokenSource();

                //Set base url for rest api
                App.BaseUrl = string.Format("{0}/rest/api/latest/", App.ServerUrl);
                var isLoginSuccess = await jiraService.LoginAsync(ServerUrl, UserName, Password, cancellationTokenSource);
                if (isLoginSuccess)
                {
                    App.IsLoggedIn = true;
                    StorageHelper.SaveUserCredential(ServerUrl, UserName, Password);                        
                    navigationService.Navigate<ProjectListViewModel>();                    
                }
            }         
            catch (HttpRequestStatusCodeException exception)
            {
                IsTaskbarVisible = false;
                if (exception.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {                    
                    dialogService.ShowDialog(AppResources.UnauthorizedMessage, AppResources.LoginFailedMessage);
                }
                else if (exception.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    dialogService.ShowDialog(AppResources.ForbiddenMessage, AppResources.LoginFailedMessage);
                }
                else
                {
                    dialogService.ShowDialog(AppResources.ConnectionErrorMessage, AppResources.LoginFailedMessage);
                }
                IsTaskbarVisible = true;
            }
            catch (Exception ex)
            {
                IsTaskbarVisible = false;
                dialogService.ShowDialog(string.Format(AppResources.FormattedErrorMessage, ex.Message), AppResources.LoginFailedMessage);
                IsTaskbarVisible = true;
            }
         
           IsDataLoaded = true;
           LoginButtonEnabled = true;
        }
        public bool ValidateUrl(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        public bool IsInputsValid()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return false;
            return true;            
        }

        private void InitializeData()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            AboutCommand = new RelayCommand(NavigateToAboutView);
            
            IsDataLoaded = true;
            LoginButtonEnabled = false;
            var credential = StorageHelper.GetUserCredential();
            if (credential != null)
            {
                ServerUrl = credential.ServerUrl;
                UserName = credential.UserName;
                Password = credential.Password;                
                Login();
            }
            else
            {
                LoginButtonEnabled = true;
            }
        }

        private bool CanLogin()
        {
            return LoginButtonEnabled;
        }

        private void NavigateToAboutView()
        {
            navigationService.Navigate<AboutViewModel>();
        }

        public void CancelLogin()
        {
            if(cancellationTokenSource != null)
            cancellationTokenSource.Cancel();
        }

        public void ClearFields()
        {
            ServerUrl = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;            
        }

        public void RemoveBackEntry()
        {
            navigationService.RemoveBackEntry();
        }
    }
}
