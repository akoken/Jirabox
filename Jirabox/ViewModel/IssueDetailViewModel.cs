using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Jirabox.ViewModel
{
    public class IssueDetailViewModel : ViewModelBase
    {
        IJiraService jiraService;
        INavigationService navigationService;

        private bool isDataLoaded;
        private Issue issue;
        private bool isOpen;
        private Thickness dynamicMargin;
        private Comment selectedComment;
        private ObservableCollection<Transition> transitions;

        public RelayCommand<Comment> ShowCommentDetailCommand { get; private set; }
        
        public RelayCommand AddCommentCommand { get; private set; }

        public RelayCommand ChangeStatusCommand { get; private set; }

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
       
        public Thickness DynamicMargin
        {
            get { return dynamicMargin; }
            set
            {
                if (dynamicMargin != value)
                {
                    dynamicMargin = value;
                    RaisePropertyChanged(() => DynamicMargin);
                }
            }
        }    

        public bool IsOpen
        {
            get { return isOpen; }
            set 
            {
                if (isOpen != value)
                {
                    isOpen = value;
                    RaisePropertyChanged(() => IsOpen);
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

        public IssueDetailViewModel(IJiraService jiraService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.navigationService = navigationService;

            ShowCommentDetailCommand = new RelayCommand<Comment>(comment => ShowCommentDetail(comment), comment => comment != null);
            AddCommentCommand = new RelayCommand(AddComment);
            ChangeStatusCommand = new RelayCommand(NavigateToChangeStatusView, CanChangeStatus);
            
            MessengerInstance.Register<RoutedEventArgs>(this, arg => 
            {                
                    SelectedComment = null;
                    IsOpen = false;                                          
            });            
        }

        private bool CanChangeStatus()
        {
            return Transitions!= null && Transitions.Count > 0;
        }

        private void NavigateToChangeStatusView()
        {
            var parameterPackage = new StatusPackage
            {
                SelectedIssue = Issue,
                Transitions = Transitions,
                SearchParameter = (SearchParameter)navigationService.GetNavigationParameter()
            };
            navigationService.Navigate<ChangeStatusViewModel>(parameterPackage);
        }

        private void ShowCommentDetail(Comment comment)
        {
            SelectedComment = comment;
            IsOpen = true;
            MessengerInstance.Send<Comment>(comment);
        }
        private void AddComment()
        {
            navigationService.Navigate<AddCommentViewModel>(Issue.ProxyKey);
        }
        public async void Initialize(string issueKey)
        {
            IsDataLoaded = false;
            IsOpen = false;
            Issue = await jiraService.GetIssueByKey(App.ServerUrl, App.UserName, App.Password, issueKey);
            Transitions =  await jiraService.GetTransitions(issueKey);
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            IsDataLoaded = true;
            Issue = null;
            IsOpen = false;
        }
    }
}
