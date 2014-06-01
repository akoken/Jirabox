using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using System.Collections.ObjectModel;

namespace Jirabox.ViewModel
{
    public class CreateIssueViewModel : ViewModelBase
    {
        private IJiraService jiraService;
        private IDialogService dialogService;
        private INavigationService navigationService;
        private bool isDataLoaded;
        private string summary;
        private string description;        
        private ObservableCollection<IssueType> issueTypes;
        private ObservableCollection<Priority> priorityList;
        private IssueType selectedIssueType;
        private Priority selectedPriority;
        private Project project;

        public RelayCommand CreateIssueCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
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
        public string Summary
        {
            get { return summary; }
            set
            {
                if (summary != value)
                {
                    summary = value;
                    RaisePropertyChanged(() => Summary);
                }
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }
     
        public ObservableCollection<IssueType> IssueTypes
        {
            get
            {
                return issueTypes;
            }
            set
            {
                if (issueTypes != value)
                {
                    issueTypes = value;
                    RaisePropertyChanged(() => IssueTypes);
                }
            }
        }
        public ObservableCollection<Priority> PriorityList
        {
            get
            {
                return priorityList;
            }
            set
            {
                if (priorityList != value)
                {
                    priorityList = value;
                    RaisePropertyChanged(() => PriorityList);
                }
            }
        }
        public IssueType SelectedIssueType
        {
            get
            {
                return selectedIssueType;
            }
            set
            {
                if (selectedIssueType != value)
                {
                    selectedIssueType = value;
                    RaisePropertyChanged(() => SelectedIssueType);
                }
            }
        }
        public Priority SelectedPriority
        {
            get
            {
                return selectedPriority;
            }
            set
            {
                if (selectedPriority != value)
                {
                    selectedPriority = value;
                    RaisePropertyChanged(() => SelectedPriority);
                }
            }
        }

        public Project Project
        {
            get
            {
                return project;
            }
            set
            {
                if (project != value)
                {
                    project = value;
                    RaisePropertyChanged(() => Project);
                }
            }
        }

        public CreateIssueViewModel(IJiraService jiraService, IDialogService dialogService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            CreateIssueCommand = new RelayCommand(CreateIssue);
            CancelCommand = new RelayCommand(NavigateToBack);                        
        }

        public async void Initialize()
        {
            Project = navigationService.GetNavigationParameter() as Project;
            IssueTypes = await jiraService.GetIssueTypesOfProject(Project.Key);
            PriorityList = await jiraService.GetPriorities();
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            Project = null;                  
            Summary = string.Empty;
            Description = string.Empty;
        }

        private async void CreateIssue()
        {
            IsDataLoaded = false;
            var request = new CreateIssueRequest();
            request.Fields.CreateIssueProject.Key = Project.Key;
            request.Fields.Description = Description;
            request.Fields.IssueType.Name = SelectedIssueType.Name;
            request.Fields.Priority.Id = SelectedPriority.Id;
            request.Fields.Summary = Summary;

            var createdIssue = await jiraService.CreateIssue(request);
            dialogService.ShowDialog(string.Format(AppResources.IssueCreatedMessage, createdIssue.Key), "Done");
            IsDataLoaded = true;
            navigationService.GoBack();
        }

        private void NavigateToBack()
        {
            CleanUp();
            navigationService.GoBack();
        }
    }
}
