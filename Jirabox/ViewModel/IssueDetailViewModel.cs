using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;

namespace Jirabox.ViewModel
{
    public class IssueDetailViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;

        private ObservableCollection<Transition> transitions;
        private Comment selectedComment;
        private bool isDataLoaded;
        private Issue issue;               

        public RelayCommand<Attachment> DownloadAttachmentCommand { get; private set; }
        
        public RelayCommand AddCommentCommand { get; private set; }

        public RelayCommand ChangeStatusCommand { get; private set; }

        public RelayCommand LogWorkCommand { get; private set; }

        public Comment SelectedComment
        {
            get { return selectedComment; }
            set
            {
                if (selectedComment != value)
                {
                    selectedComment = value;
                    RaisePropertyChanged(() => SelectedComment);
                }
            }
        }
     
        public Issue Issue 
        { 
            get
            {
                return issue;
            }
            set
            {
                if (issue != value)
                {
                    issue = value;
                    RaisePropertyChanged(() => Issue);
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

        public ObservableCollection<Transition> Transitions
        {
            get
            {
                return transitions;
            }
            set
            {
                if (transitions != value)
                {
                    transitions = value;
                    RaisePropertyChanged(() => Transitions);
                    ChangeStatusCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IssueDetailViewModel(IJiraService jiraService, INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;
          
            AddCommentCommand = new RelayCommand(AddComment);
            ChangeStatusCommand = new RelayCommand(NavigateToChangeStatusView, CanChangeStatus);
            LogWorkCommand = new RelayCommand(NavigateToLogWorkView);            
        }

        private void NavigateToLogWorkView()
        {
            navigationService.Navigate<LogWorkViewModel>((object)Issue.ProxyKey);
        }       

        private bool CanChangeStatus()
        {
            return Transitions!= null && Transitions.Count > 0;
        }

        private void NavigateToChangeStatusView()
        {
            var parameter = navigationService.GetNavigationParameter();
            var parameterPackage = new StatusPackage
            {
                SelectedIssue = Issue,
                Transitions = Transitions,              
            };

            if (parameter is SearchParameter)
                parameterPackage.SearchParameter = (SearchParameter)parameter;            

                navigationService.Navigate<ChangeStatusViewModel>(parameterPackage);         
        }
    
        private void AddComment()
        {
            navigationService.Navigate<AddCommentViewModel>(Issue.ProxyKey);
        }
        public async void Initialize(string issueKey)
        {
            IsDataLoaded = false;            
            Issue = await jiraService.GetIssueByKey(App.ServerUrl, App.UserName, App.Password, issueKey);
            Transitions =  await jiraService.GetTransitions(issueKey);
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            IsDataLoaded = true;
            Issue = null;            
        }
    }
}
