using GalaSoft.MvvmLight.Ioc;
using Jirabox.Core;
using Jirabox.Core.Contracts;
using Jirabox.Services;
using Microsoft.Practices.ServiceLocation;

namespace Jirabox.ViewModel
{   
    public class ViewModelLocator
    {       
        public ViewModelLocator()
        {            
            IoC.Register<IHttpManager, HttpManager>();
            IoC.Register<IJsonHttpClient, JsonHttpClient>();
            IoC.Register<IJiraService, JiraService>();
            IoC.Register<INavigationService, NavigationService>();
            IoC.Register<IDialogService, DialogService>();
            IoC.Register<ICacheService, CacheService>();   
            
            IoC.Register<LoginViewModel>();
            IoC.Register<ProjectListViewModel>();
            IoC.Register<ProjectDetailViewModel>();            
            IoC.Register<IssueDetailViewModel>();    
            IoC.Register<AddCommentViewModel>();
            IoC.Register<CreateIssueViewModel>();
            IoC.Register<SearchResultViewModel>();
            IoC.Register<UserProfileViewModel>();
            IoC.Register<AboutViewModel>();
            IoC.Register<SettingsViewModel>();
            IoC.Register<ChangeStatusViewModel>();
            IoC.Register<LogWorkViewModel>();
        }

        public ProjectListViewModel ProjectListViewModel
        {
            get
            {
                return IoC.Resolve<ProjectListViewModel>();
            }
        }

        public ProjectDetailViewModel ProjectDetailViewModel
        {
            get
            {
                return IoC.Resolve<ProjectDetailViewModel>();
            }
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return IoC.Resolve<LoginViewModel>();
            }
        }       

        public IssueDetailViewModel IssueDetailViewModel
        {
            get
            {
                return IoC.Resolve<IssueDetailViewModel>();
            }
        }
       
        public AddCommentViewModel AddCommentViewModel
        {
            get
            {
                return IoC.Resolve<AddCommentViewModel>();
            }
        }

        public CreateIssueViewModel CreateIssueViewModel
        {
            get
            {
                return IoC.Resolve<CreateIssueViewModel>();
            }
        }

        public SearchResultViewModel SearchResultViewModel
        {
            get
            {
                return IoC.Resolve<SearchResultViewModel>();
            }
        }

        public UserProfileViewModel UserProfileViewModel
        {
            get
            {
                return IoC.Resolve<UserProfileViewModel>();
            }
        }

        public AboutViewModel AboutViewModel
        {
            get
            {
                return IoC.Resolve<AboutViewModel>();
            }
        }
        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return IoC.Resolve<SettingsViewModel>();
            }
        }

        public ChangeStatusViewModel ChangeStatusViewModel
        {
            get
            {
                return IoC.Resolve<ChangeStatusViewModel>();
            }
        }

        public LogWorkViewModel LogWorkViewModel
        {
            get
            {
                return IoC.Resolve<LogWorkViewModel>();
            }
        }

        public static void Cleanup()
        {
          
        }
    }
}