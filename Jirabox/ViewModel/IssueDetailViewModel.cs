using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Jirabox.ViewModel
{
    public class IssueDetailViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;        
        private readonly IJiraService jiraService;

        private ObservableCollection<Transition> transitions;
        private Comment selectedComment;
        private bool isDataLoaded;
        private string isLoadingText;
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

        public string IsLoadingText
        {
            get
            {
                return isLoadingText;
            }
            set
            {
                if (isLoadingText != value)
                {
                    isLoadingText = value;
                    RaisePropertyChanged(() => IsLoadingText);
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

        public IssueDetailViewModel(IJiraService jiraService, INavigationService navigationService)
        {
            this.navigationService = navigationService;            
            this.jiraService = jiraService;
          
            AddCommentCommand = new RelayCommand(AddComment);
            ChangeStatusCommand = new RelayCommand(NavigateToChangeStatusView, CanChangeStatus);
            LogWorkCommand = new RelayCommand(NavigateToLogWorkView);
            DownloadAttachmentCommand = new RelayCommand<Attachment>(async (t) => await DownloadAttachment(t));
        }

        private async Task DownloadAttachment(Attachment attachment)
        {
            IsLoadingText = AppResources.DownloadingMessage;
            IsDataLoaded = false;
            var isDownloaded = await jiraService.DownloadAttachment(attachment.Url, attachment.FileName);
            IsDataLoaded = true;
        }

        private void NavigateToLogWorkView()
        {
            if (Issue != null && !String.IsNullOrEmpty(Issue.ProxyKey))
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
            IsLoadingText = AppResources.GettingIssueDetailsMessage;
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
