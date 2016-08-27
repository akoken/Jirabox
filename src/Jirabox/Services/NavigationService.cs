using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Jirabox.Services
{
    /// <summary> 
    /// The navigation service. 
    /// </summary> 
    public class NavigationService : INavigationService
    {
        private static readonly Dictionary<Type, string> ViewModelRouting = new Dictionary<Type, string>
        { 
                                                                          { 
                                                                              typeof(LoginViewModel), ViewNames.LoginView 
                                                                          }, 
                                                                          { 
                                                                              typeof(ProjectListViewModel), ViewNames.ProjectListView
                                                                          },
                                                                          {
                                                                              typeof(ProjectDetailViewModel), ViewNames.ProjectDetailView 
                                                                          },
                                                                          {
                                                                              typeof(SearchResultViewModel), ViewNames.SearchResultView
                                                                          },
                                                                          {
                                                                              typeof(IssueDetailViewModel), ViewNames.IssueDetailView
                                                                          },
                                                                          {
                                                                              typeof(AddCommentViewModel), ViewNames.AddCommentView
                                                                          },
                                                                          {
                                                                              typeof(CreateIssueViewModel), ViewNames.CreateIssueView
                                                                          },
                                                                          {
                                                                              typeof(UserProfileViewModel), ViewNames.UserProfileView
                                                                          },
                                                                          {
                                                                              typeof(AboutViewModel), ViewNames.AboutView
                                                                          },
                                                                          {
                                                                              typeof(SettingsViewModel), ViewNames.SettingsView
                                                                          },
                                                                          {
                                                                              typeof(ChangeStatusViewModel), ViewNames.ChangeStatusView
                                                                          },
                                                                          {
                                                                              typeof(LogWorkViewModel), ViewNames.LogWorkView
                                                                          }
                                                                    };        
       
        public bool CanGoBack
        {
            get
            {
                return RootFrame.CanGoBack;
            }
        }
       
        private TransitionFrame RootFrame
        {
            get { return Application.Current.RootVisual as TransitionFrame; }
        }

        public object NavigationParameter
        {
            get
            {
                return App.NavigationParameter;
            }
            set
            {
                App.NavigationParameter = value;
            }
        }
      
        public void GoBack()
        {
            RootFrame.GoBack();
        }
      
        public void Navigate<TDestinationViewModel>(string parameter)
        {
            var navParameter = string.Empty;
            if (parameter != null)
            {                
                navParameter = "?param=" + parameter;
            }

            if (ViewModelRouting.ContainsKey(typeof(TDestinationViewModel)))
            {
                var page = ViewModelRouting[typeof(TDestinationViewModel)];

                RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            }
        }

        public void Navigate<TDestinationViewModel>(object parameter)
        {            
            if (parameter != null)
            {
                NavigationParameter = parameter;
            }

            if (ViewModelRouting.ContainsKey(typeof(TDestinationViewModel)))
            {
                var page = ViewModelRouting[typeof(TDestinationViewModel)];

                RootFrame.Navigate(new Uri("/" + page, UriKind.Relative));
            }
        }

        public void RemoveBackEntry()
        {
            RootFrame.RemoveBackEntry();
        }

        public object GetNavigationParameter()
        {
            return NavigationParameter;
        }
    }
}
