using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Core.ExceptionExtension;
using Jirabox.Resources;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IDialogService dialogService;
        private IJiraService jiraService;
        private string serverUrl;
        private string userName;        
        private string password;
        private bool isDataLoaded;
        private bool isRememberMe;
        private CancellationTokenSource cancellationTokenSource = null;

        public RelayCommand LoginCommand { get; private set; }        

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

        public bool IsRememberMe
        {
            get { return isRememberMe; }
            set
            {
                if (value != isRememberMe)
                {
                    isRememberMe = value;
                    RaisePropertyChanged(() => IsRememberMe);
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

            if (!IsInputsValid())
            {
                IsDataLoaded = true;
                dialogService.ShowDialog(AppResources.UnauthorizedMessage, "Login failed");
                return;
            }

            if (!ValidateUrl(ServerUrl))
            {
                IsDataLoaded = true;
                dialogService.ShowDialog(AppResources.InvalidServerUrl, "Login failed");                
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
                    if (IsRememberMe)
                        StorageHelper.SaveUserCredential(ServerUrl, UserName, Password);
                    else
                        StorageHelper.ClearUserCredential();

                    navigationService.Navigate<ProjectListViewModel>();
                }
            }         
            catch (HttpRequestStatusCodeException exception)
            {
                if (exception.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    dialogService.ShowDialog(AppResources.UnauthorizedMessage, "Login failed");
                }
                else if (exception.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    dialogService.ShowDialog(AppResources.ForbiddenMessage, "Login failed");
                }
                else
                {
                    dialogService.ShowDialog(AppResources.ConnectionErrorMessage, "Login failed");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowDialog(string.Format(AppResources.FormattedErrorMessage, ex.Message), "Login failed");
            }
         
           IsDataLoaded = true;
        }
        public bool ValidateUrl(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private bool IsInputsValid()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return false;
            return true;            
        }

        private void InitializeData()
        {
            LoginCommand = new RelayCommand(Login);
            IsDataLoaded = true;

            var credential = StorageHelper.GetUserCredential();
            if (credential != null)
            {
                ServerUrl = credential.ServerUrl;
                UserName = credential.UserName;
                Password = credential.Password;
                IsRememberMe = true;
            }
        }

        public void CancelLogin()
        {
            if(cancellationTokenSource != null)
            cancellationTokenSource.Cancel();
        }
    }
}
