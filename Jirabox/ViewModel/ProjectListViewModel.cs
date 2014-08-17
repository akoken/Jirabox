using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Linq;
using Jirabox.Resources;
using System.Threading;

namespace Jirabox.ViewModel
{
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ICacheDataService cacheDataService;
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;

        private bool isDataLoaded;
        private bool isGroupingEnabled;
        private BitmapImage displayPicture;
        private List<AlphaKeyGroup<Project>> groupedProjects;
        private ObservableCollection<Project> flatProjects;

        public RelayCommand<Project> ProjectDetailCommand { get; private set; }
        public RelayCommand ShowUserInformationCommand { get; private set; }
        public RelayCommand ShowAssignedIssuesCommand { get; private set; }
        public RelayCommand ShowIssuesReportedByMeCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand ShowAboutViewCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                isDataLoaded = value;
                RaisePropertyChanged(() => IsDataLoaded);
            }
        }

        public bool IsGroupingEnabled
        {
            get { return isGroupingEnabled; }
            set
            {
                if (isGroupingEnabled != value)
                {
                    isGroupingEnabled = value;
                    RaisePropertyChanged(() => IsGroupingEnabled);
                }
            }
        }


        public BitmapImage DisplayPicture
        {
            get { return displayPicture; }
            set
            {
                if (displayPicture != value)
                {
                    displayPicture = value;
                    RaisePropertyChanged(() => DisplayPicture);
                }
            }
        }

        public List<AlphaKeyGroup<Project>> GroupedProjects
        {
            get { return groupedProjects; }
            set
            {
                groupedProjects = value;
                RaisePropertyChanged(() => GroupedProjects);
            }
        }

        public ObservableCollection<Project> FlatProjects
        {
            get { return flatProjects; }
            set
            {
                flatProjects = value;
                RaisePropertyChanged(() => FlatProjects);
            }
        }

        public ProjectListViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService, ICacheDataService cacheDataService)
        {
            this.navigationService = navigationService;
            this.cacheDataService = cacheDataService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;

            ProjectDetailCommand = new RelayCommand<Project>(project => NavigateToProjectDetail(project), project => project != null);
            ShowUserInformationCommand = new RelayCommand(NavigateToUserProfile);
            ShowAssignedIssuesCommand = new RelayCommand(NavigateToAssignedIssues);
            ShowIssuesReportedByMeCommand = new RelayCommand(NavigateToIssuesReportedByMe);
            ShowSettingsCommand = new RelayCommand(NavigateToSettingsView);
            ShowAboutViewCommand = new RelayCommand(NavigateToAboutView);
            SearchCommand = new RelayCommand<string>(searchText => NavigateToSearchResults(searchText));
            RefreshCommand = new RelayCommand(async () => await Refresh());
            LogoutCommand = new RelayCommand(Logout);

            MessengerInstance.Register<bool>(this, (isEnabled) =>
            {
                IsGroupingEnabled = isEnabled;
            });          
        }
        private void Logout()
        {
            var messageBox = dialogService.ShowPromptDialog(AppResources.LogoutWarningMessage, AppResources.LogoutPromptMessage, AppResources.LogoutPromptCaption);
            messageBox.Dismissed += async(s1, e1) =>
            {
                if (e1.Result == Microsoft.Phone.Controls.CustomMessageBoxResult.LeftButton)
                {
                    IsDataLoaded = false;
                    StorageHelper.ClearUserCredential();
                    await cacheDataService.ClearCacheData();
                    DeleteSecondaryTiles();
                    App.IsLoggedIn = false;
                    IsDataLoaded = true;

                    SimpleIoc.Default.GetInstance<LoginViewModel>().ClearFields();
                    navigationService.Navigate<LoginViewModel>();                        
                }                
            };            
        }

        private void DeleteSecondaryTiles()
        {
            var tiles = ShellTile.ActiveTiles.Where(tile => tile.NavigationUri.ToString().Contains("/View/ProjectDetailView.xaml?Key="));
            tiles.ToList().ForEach(tile =>
            {
                tile.Delete();
            });
        }       
        public async Task InitializeData(bool withoutCache = false)
        {
            GroupedProjects = null;
            FlatProjects = null;
            IsDataLoaded = false;
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            jiraService.GetUserProfileAsync(App.UserName).ContinueWith(task =>
            {                
                DisplayPicture = jiraService.GetDisplayPicture(App.UserName);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, taskScheduler);

            var projects = await jiraService.GetProjects(App.ServerUrl, App.UserName, App.Password, withoutCache);
            GroupedProjects = AlphaKeyGroup<Project>.CreateGroups(projects, System.Threading.Thread.CurrentThread.CurrentUICulture, (Project s) => { return s.Name; }, true);
            FlatProjects = projects;
            
            IsGroupingEnabled = true;
            IsGroupingEnabled = new IsolatedStorageProperty<bool>(Settings.IsGroupingEnabled, true).Value;
            IsDataLoaded = true;
        }
        public void RemoveBackEntry()
        {
            navigationService.RemoveBackEntry();
        }
        public void NavigateToLoginView()
        {
            navigationService.Navigate<LoginViewModel>();            
        }
        private async Task Refresh()
        {
            await InitializeData(true);
        }
        private void NavigateToProjectDetail(Project project)
        {
            navigationService.Navigate<ProjectDetailViewModel>(project.Key);
        }
        private void NavigateToUserProfile()
        {
            navigationService.Navigate<UserProfileViewModel>();
        }
        private void NavigateToAssignedIssues()
        {
            var searchCriteria = new SearchParameter { IsAssignedToMe = true };
            navigationService.Navigate<SearchResultViewModel>(searchCriteria);
        }
        private void NavigateToIssuesReportedByMe()
        {
            var searchCriteria = new SearchParameter { IsReportedByMe = true };
            navigationService.Navigate<SearchResultViewModel>(searchCriteria);
        }
        private void NavigateToSettingsView()
        {
            navigationService.Navigate<SettingsViewModel>();
        }
        private void NavigateToAboutView()
        {
            navigationService.Navigate<AboutViewModel>();
        }
        private void NavigateToSearchResults(string searchText)
        {
            var searchCriteria = new SearchParameter { SearchText = searchText };
            navigationService.Navigate<SearchResultViewModel>(searchCriteria);
        }
    }
}
