﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Jirabox.ViewModel
{
    public class ProjectListViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IDialogService dialogService;
        private IJiraService jiraService;
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
        public RelayCommand ShowAboutPageCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }

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
                if (groupedProjects != value)
                {
                    groupedProjects = value;
                    RaisePropertyChanged(() => GroupedProjects);
                }
            }
        }

        public ObservableCollection<Project> FlatProjects
        {
            get { return flatProjects; }
            set
            {
                if (flatProjects != value)
                {
                    flatProjects = value;
                    RaisePropertyChanged(() => FlatProjects);
                }
            }
        }

        public ProjectListViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;

            ProjectDetailCommand = new RelayCommand<Project>(project => NavigateToProjectDetail(project), project => project != null);
            ShowUserInformationCommand = new RelayCommand(NavigateToUserProfile);
            ShowAssignedIssuesCommand = new RelayCommand(NavigateToAssignedIssues);
            ShowIssuesReportedByMeCommand = new RelayCommand(NavigateToIssuesReportedByMe);
            ShowSettingsCommand = new RelayCommand(NavigateToSettingsView);
            ShowAboutPageCommand = new RelayCommand(NavigateToAboutView);
            SearchCommand = new RelayCommand<string>(searchText => NavigateToSearchResults(searchText));

            if (!IsInDesignMode)
            {
                InitializeData();

                //Remove login view from navigation history
                navigationService.RemoveBackEntry();
            }

            MessengerInstance.Register<bool>(this, (isEnabled) =>
            {
                IsGroupingEnabled = isEnabled;
            });
        }

        private async void InitializeData()
        {
            IsDataLoaded = false;
            await jiraService.GetUserProfileAsync(App.UserName);
            var projects = await jiraService.GetProjects(App.ServerUrl, App.UserName, App.Password);

            GroupedProjects = AlphaKeyGroup<Project>.CreateGroups(projects, System.Threading.Thread.CurrentThread.CurrentUICulture, (Project s) => { return s.Name; }, true);
            FlatProjects = projects;
            DisplayPicture = jiraService.GetDisplayPicture(App.UserName);

            IsGroupingEnabled = true;
            IsGroupingEnabled = new IsolatedStorageProperty<bool>(Settings.IsGroupingEnabled, true).Value;
            IsDataLoaded = true;
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