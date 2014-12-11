using GalaSoft.MvvmLight.Ioc;
using Jirabox.Core.Contracts;
using Jirabox.Services;
using Microsoft.Practices.ServiceLocation;

namespace Jirabox.ViewModel
{   
    public class ViewModelLocator
    {       
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IHttpManager, HttpManager>();
            SimpleIoc.Default.Register<IJiraService, JiraService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<ICacheService, CacheService>();   
       
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ProjectListViewModel>();
            SimpleIoc.Default.Register<ProjectDetailViewModel>();            
            SimpleIoc.Default.Register<IssueDetailViewModel>();    
            SimpleIoc.Default.Register<AddCommentViewModel>();
            SimpleIoc.Default.Register<CreateIssueViewModel>();
            SimpleIoc.Default.Register<SearchResultViewModel>();
            SimpleIoc.Default.Register<UserProfileViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<ChangeStatusViewModel>();
            SimpleIoc.Default.Register<LogWorkViewModel>();
        }

        public ProjectListViewModel ProjectListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProjectListViewModel>();
            }
        }

        public ProjectDetailViewModel ProjectDetailViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProjectDetailViewModel>();
            }
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }       

        public IssueDetailViewModel IssueDetailViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IssueDetailViewModel>();
            }
        }
       
        public AddCommentViewModel AddCommentViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddCommentViewModel>();
            }
        }

        public CreateIssueViewModel CreateIssueViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateIssueViewModel>();
            }
        }

        public SearchResultViewModel SearchResultViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchResultViewModel>();
            }
        }

        public UserProfileViewModel UserProfileViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserProfileViewModel>();
            }
        }

        public AboutViewModel AboutViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }
        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public ChangeStatusViewModel ChangeStatusViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChangeStatusViewModel>();
            }
        }

        public LogWorkViewModel LogWorkViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LogWorkViewModel>();
            }
        }

        public static void Cleanup()
        {
          
        }
    }
}