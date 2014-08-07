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
        private int selectedIssueTypeIndex = -1;
        private int selectedPriorityIndex = -1;
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
        public int SelectedIssueTypeIndex
        {
            get
            {
                return selectedIssueTypeIndex;
            }
            set
            {
                if (selectedIssueTypeIndex != value)
                {
                    selectedIssueTypeIndex = value;
                    RaisePropertyChanged(() => SelectedIssueTypeIndex);
                }
            }
        }
        public int SelectedPriorityIndex
        {
            get
            {
                return selectedPriorityIndex;
            }
            set
            {
                if (selectedPriorityIndex != value)
                {
                    selectedPriorityIndex = value;
                    RaisePropertyChanged(() => SelectedPriorityIndex);
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
            CreateIssueCommand = new RelayCommand(CreateIssue, CanCreateIssue);
            CancelCommand = new RelayCommand(NavigateToBack);                        
        }

        private bool CanCreateIssue()
        {
            return Project != null;
        }

        public async void Initialize()
        {
            Project = navigationService.GetNavigationParameter() as Project;
            if (Project == null)
            {
                dialogService.ShowDialog(AppResources.CreateIssueNullProjectMessage, AppResources.Error);
                IsDataLoaded = true;
                return;
            }
            CreateIssueCommand.RaiseCanExecuteChanged();
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
            request.Fields.IssueType.Name = IssueTypes[SelectedIssueTypeIndex].Name;
            request.Fields.Priority.Id = PriorityList[SelectedPriorityIndex].Id;
            request.Fields.Summary = Summary;

            var createdIssue = await jiraService.CreateIssue(request);
            dialogService.ShowDialog(string.Format(AppResources.IssueCreatedMessage, createdIssue.Key), AppResources.Done);
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
