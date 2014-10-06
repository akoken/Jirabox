using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Jirabox.ViewModel
{
    public class ProjectDetailViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IDialogService dialogService;
        private IJiraService jiraService;
        private string key;
        private bool isDataLoaded;
        private Project project;
        private ObservableCollection<Issue> issues;

        public RelayCommand<Issue> ShowIssueDetailCommand { get; private set; }
        public RelayCommand CreateIssueCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand PinToStartScreenCommand { get; private set; }
        

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

        public Project Project
        {
            get { return project; }
            set
            {
                if (project != value)
                {
                    project = value;
                    RaisePropertyChanged(() => Project);
                }
            }
        }

        public string Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    RaisePropertyChanged(() => Key);
                }
            }
        }

        public ObservableCollection<Issue> Issues
        {
            get
            {
                return issues;
            }
            set
            {
                if (issues != value)
                {
                    issues = value;
                    RaisePropertyChanged(() => Issues);
                }
            }
        }

        public ProjectDetailViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;

            ShowIssueDetailCommand = new RelayCommand<Issue>(issue => NavigateToIssueDetailView(issue), issue => issue != null);
            CreateIssueCommand = new RelayCommand(NavigateToCreateIssueView);
            RefreshCommand = new RelayCommand(RefreshIssues);
            PinToStartScreenCommand = new RelayCommand(PinToStartScreen);
        }

        public async void Initialize(string projectKey, bool withoutCache = false)
        {
            MessengerInstance.Register<bool>(this, AppResources.CreateIssueToken, isIssueCreated =>
            {
                if (isIssueCreated)
                {
                    RefreshIssues();
                }
            });

            IsDataLoaded = false;
            Project = await jiraService.GetProjectByKey(App.ServerUrl, App.UserName, App.Password, projectKey, withoutCache);
            Issues = await jiraService.GetIssuesByProjectKey(App.ServerUrl, App.UserName, App.Password, projectKey, withoutCache);
            Key = projectKey;
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            Project = null;
            Issues = null;
            Key = null;
        }

        private void PinToStartScreen()
        {
            var imageFolder = @"\Shared\ShellContent";
            var projectKey = string.Format("{0}.png", Project.Key.Trim());
            var avatarPath = Path.Combine(imageFolder, projectKey);
            var tileUri = new Uri(string.Format("/View/ProjectDetailView.xaml?Key={0}", Project.Key), UriKind.Relative);

            //Create project tile
            if(!DoesTileAlreadyExist(tileUri.ToString()))
            ShellTile.Create(tileUri, new StandardTileData
            {                
                BackgroundImage = new Uri(@"isostore:" + avatarPath, UriKind.Absolute),
                Title = Project.Key,
                BackContent = Project.Name,                             
            });
        }

        private void RefreshIssues()
        {            
            Initialize(Key, true);
        }  
 
        private void NavigateToIssueDetailView(Issue selectedIssue)
        {
            navigationService.Navigate<IssueDetailViewModel>(selectedIssue.ProxyKey);
        }

        private void NavigateToCreateIssueView()
        {
            navigationService.Navigate<CreateIssueViewModel>(Project);
        }
       
        private bool DoesTileAlreadyExist(string path)
        {
           var secondaryTiles = ShellTile.ActiveTiles.Where(tile => tile.NavigationUri.ToString() == path);
           return secondaryTiles.Count() > 0;
        }
    }
}
