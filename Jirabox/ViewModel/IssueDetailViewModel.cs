using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Core.ExceptionExtension;
using Jirabox.Model;
using System;
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
        public RelayCommand<Comment> ShowCommentDetailCommand { get; private set; }
        public RelayCommand AddCommentCommand { get; private set; }
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

        public IssueDetailViewModel(IJiraService jiraService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.navigationService = navigationService;
            ShowCommentDetailCommand = new RelayCommand<Comment>(comment => ShowCommentDetail(comment), comment => comment != null);
            AddCommentCommand = new RelayCommand(AddComment);
            MessengerInstance.Register<RoutedEventArgs>(this, arg => 
            {                
                    SelectedComment = null;
                    IsOpen = false;                                          
            });            
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
