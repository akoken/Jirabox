using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jirabox.ViewModel
{
    public class CreateIssueViewModel : ViewModelBase
    {
        private readonly IJiraService jiraService;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        private bool isDataLoaded;
        private string loadingText;
        private string summary;
        private string description;        
        private int selectedPriorityIndex = -1;
        private int selectedIssueTypeIndex = -1;
        private bool isTaskbarVisible = true;

        private ObservableCollection<IssueType> issueTypes;
        private ObservableCollection<Priority> priorityList;
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

        public string LoadingText
        {
            get { return loadingText; }
            set
            {
                if (loadingText != value)
                {
                    loadingText = value;
                    RaisePropertyChanged(() => LoadingText);
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
            CreateIssueCommand = new RelayCommand(async()=> await CreateIssue(), CanCreateIssue);
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
                IsTaskbarVisible = false;
                dialogService.ShowDialog(AppResources.CreateIssueNullProjectMessage, AppResources.Error);
                IsTaskbarVisible = true;
                IsDataLoaded = true;
                return;
            }
            CreateIssueCommand.RaiseCanExecuteChanged();
            IssueTypes = await jiraService.GetIssueTypesOfProject(Project.Key);
            PriorityList = await jiraService.GetPriorities();
            LoadingText = AppResources.LoadingMessage;
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            Project = null;                  
            Summary = string.Empty;
            Description = string.Empty;
        }

        private async Task CreateIssue()
        {
            if (!ValidateSummary()) return;
            if (!ValidateIssueTypes()) return;
            if (!ValidatePriorities()) return;

            var request = new CreateIssueRequest();
            request.Fields.CreateIssueProject.Key = Project.Key;
            request.Fields.Description = Description;
            request.Fields.IssueType.Name = IssueTypes[SelectedIssueTypeIndex].Name;
            request.Fields.Priority.Id = PriorityList[SelectedPriorityIndex].Id;
            request.Fields.Summary = Summary;
            LoadingText = AppResources.CreatingIssueMessage;

            IsDataLoaded = false;
            var createdIssue = await jiraService.CreateIssue(request);
            if (createdIssue == null)
            {
                IsTaskbarVisible = false;
                dialogService.ShowDialog(AppResources.CreateIssueErrorMessage, AppResources.Error);
                IsTaskbarVisible = true;
            }
            else
            {
                IsTaskbarVisible = false;
                dialogService.ShowDialog(string.Format(AppResources.IssueCreatedMessage, createdIssue.Key), AppResources.Done);
                IsTaskbarVisible = true;
                MessengerInstance.Send(true, AppResources.CreateIssueToken);
                navigationService.GoBack();
            }

            IsDataLoaded = true;
        }

        private void NavigateToBack()
        {
            CleanUp();
            navigationService.GoBack();
        }

        public bool ValidateSummary()
        {
            if (!string.IsNullOrEmpty(Summary)) return true;

            IsTaskbarVisible = false;
            dialogService.ShowDialog(AppResources.CreateIssueSummaryValidationErrorMessage, AppResources.Warning);
            IsTaskbarVisible = true;

            return false;
        }

        public bool ValidateIssueTypes()
        {
            if (IssueTypes.Any()) return true;

            IsTaskbarVisible = false;
            dialogService.ShowDialog(AppResources.CreateIssueIssueTypeValidationErrorMessage, AppResources.Warning);
            IsTaskbarVisible = true;

            return false;
        }

        public bool ValidatePriorities()
        {
            if (PriorityList.Any()) return true;

            IsTaskbarVisible = false;
            dialogService.ShowDialog(AppResources.CreateIssuePriorityValidationErrorMessage, AppResources.Warning);
            IsTaskbarVisible = true;

            return false;
        }
    }
}
