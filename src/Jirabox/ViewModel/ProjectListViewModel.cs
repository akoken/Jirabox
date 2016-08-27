using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Jirabox.Common;
using Jirabox.Common.Extensions;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Jirabox.ViewModel
{
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ICacheService cacheService;
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;

        private bool isDataLoaded;
        private bool isGroupingEnabled;
        private bool isFavouriteExist;
        private bool isTaskbarVisible = true;

        private BitmapImage displayPicture;
        private List<AlphaKeyGroup<Project>> groupedProjects;
        private ObservableCollection<Favourite> favorites;
        private ObservableCollection<Project> flatProjects;

        public RelayCommand<Project> ProjectDetailCommand { get; private set; }
        public RelayCommand ShowUserInformationCommand { get; private set; }
        public RelayCommand ShowAssignedIssuesCommand { get; private set; }
        public RelayCommand ShowIssuesReportedByMeCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand ShowAboutViewCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }        
        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<Favourite> SearchWithFavouriteCommand { get; set; }

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                isDataLoaded = value;
                RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
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
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsFavouriteExist
        {
            get { return isFavouriteExist; }
            set
            {
                if (isFavouriteExist != value)
                {
                    isFavouriteExist = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public List<AlphaKeyGroup<Project>> GroupedProjects
        {
            get { return groupedProjects; }
            set
            {
                groupedProjects = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Project> FlatProjects
        {
            get { return flatProjects; }
            set
            {
                flatProjects = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Favourite> Favourites
        {
            get { return favorites; }
            set
            {
                if (favorites != value)
                {
                    favorites = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ProjectListViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService, ICacheService cacheDataService)
        {
            this.navigationService = navigationService;
            cacheService = cacheDataService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;

            ProjectDetailCommand = new RelayCommand<Project>(NavigateToProjectDetail, project => project != null);
            ShowUserInformationCommand = new RelayCommand(NavigateToUserProfile);
            ShowAssignedIssuesCommand = new RelayCommand(NavigateToAssignedIssues);
            ShowIssuesReportedByMeCommand = new RelayCommand(NavigateToIssuesReportedByMe);
            ShowSettingsCommand = new RelayCommand(NavigateToSettingsView);
            ShowAboutViewCommand = new RelayCommand(NavigateToAboutView);
            SearchCommand = new RelayCommand<string>(NavigateToSearchResults);
            RefreshCommand = new RelayCommand(async () => await Refresh());
            LogoutCommand = new RelayCommand(Logout);
            SearchWithFavouriteCommand = new RelayCommand<Favourite>(NavigateToSearchResults, favourite=> favourite != null);

            MessengerInstance.Register<bool>(this, isEnabled =>
            {
                IsGroupingEnabled = isEnabled;
            });          
        }
        private void Logout()
        {
            IsTaskbarVisible = false;
            var messageBox = dialogService.ShowPromptDialog(AppResources.LogoutWarningMessage, AppResources.LogoutPromptMessage, AppResources.LogoutPromptCaption);
          
            messageBox.Dismissed += (s1, e1) =>
            {
                if (e1.Result == Microsoft.Phone.Controls.CustomMessageBoxResult.LeftButton)
                {
                    IsDataLoaded = false;
                    StorageHelper.ClearUserCredential();
                    cacheService.ClearCache();
                    LiveTileManager.DeleteAllSecondaryTiles();
                    App.IsLoggedIn = false;
                    IsDataLoaded = true;

                    SimpleIoc.Default.GetInstance<LoginViewModel>().ClearFields();
                    navigationService.Navigate<LoginViewModel>();                        
                }
                IsTaskbarVisible = true;
            };            
        }
       
        public async Task InitializeData(bool withoutCache = false)
        {
            GroupedProjects = null;
            FlatProjects = null;
            IsDataLoaded = false;            

            await jiraService.GetUserProfileAsync(App.UserName);
            DisplayPicture = jiraService.GetDisplayPicture(App.UserName).ToBitmapImage();           

            var projects = await jiraService.GetProjects(App.ServerUrl, App.UserName, App.Password, withoutCache);
            GroupedProjects = AlphaKeyGroup<Project>.CreateGroups(projects, System.Threading.Thread.CurrentThread.CurrentUICulture, s => s.Name, true);
            FlatProjects = projects;
            Favourites = await jiraService.GetFavourites();
            IsFavouriteExist = Favourites.Count > 0;
            
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

        public void NavigateToAssignedIssues()
        {
            var searchCriteria = new SearchParameter { IsAssignedToMe = true };
            navigationService.Navigate<SearchResultViewModel>(searchCriteria);
        }

        public void NavigateToIssuesReportedByMe()
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

        private void NavigateToSearchResults(Favourite favorite)
        {
            var searchCriteria = new SearchParameter { IsFavourite = true, JQL = favorite.JQL };
            navigationService.Navigate<SearchResultViewModel>(searchCriteria);
        }
    }
}
