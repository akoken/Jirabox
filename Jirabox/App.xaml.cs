using BugSense;
using BugSense.Core.Model;
using Jirabox.Common;
using Jirabox.Model;
using Jirabox.Resources;
using Jirabox.Services;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Jirabox
{
    public partial class App : Application
    {
        #region Properties
        public static object NavigationParameter { get; set; }
        public static string ServerUrl { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string DisplayName { get; set; }
        public static string BaseUrl { get; set; }
        public static User User { get; set; }
        public static bool IsLoggedIn { get; set; }
        public static PhoneApplicationFrame RootFrame { get; private set; } 
        #endregion

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            //Initialize Bug Sense Logging            
            BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), RootFrame, "72335f35");
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;            

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {               
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            //Set checkbox check color
            (App.Current.Resources["PhoneRadioCheckBoxCheckBrush"] as SolidColorBrush).Color = (App.Current.Resources["JiraboxSolidColorBrush"] as SolidColorBrush).Color;

        }
     
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {               
            //Clear cache data
            var cacheService = new CacheService();
            cacheService.ClearCache();

            //Clear image data
            var cacheSetting = new IsolatedStorageProperty<bool>("ClearImageCache", false);
            if (cacheSetting.Value)
            {
                StorageHelper.ClearCache();
                cacheSetting.Value = false;
            }
        }
   
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {           
        }
      
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {            
        }
                
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }
        
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {                
                Debugger.Break();
            }
        }
        
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {                
                Debugger.Break();
            }

            var extras = BugSenseHandler.Instance.CrashExtraData;
            extras.Add(new CrashExtraData
            {
                Key = "Method",
                Value = "Application_UnhandledException"
            });           
            BugSenseHandler.Instance.LogException(e.ExceptionObject, extras);
            e.Handled = true;
        }

        #region Phone application initialization
        
        private bool phoneApplicationInitialized = false;
        
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;
            
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            IsLoggedIn = false;

            //Check user is authenticated
            var credential = StorageHelper.GetUserCredential();
            if (credential != null)
            {
                IsLoggedIn = true;
                App.ServerUrl = credential.ServerUrl;
                App.UserName = credential.UserName;
                App.Password = credential.Password;
                App.BaseUrl = string.Format("{0}/rest/api/latest/", App.ServerUrl);
                RootFrame.Navigate(new Uri("/View/ProjectListView.xaml", UriKind.Relative));
            }
            else
            {
                RootFrame.Navigate(new Uri("/View/LoginView.xaml", UriKind.Relative));
            }
            
        }


        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion
      
        private void InitializeLanguage()
        {
            try
            {               
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);            
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {             
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                throw;
            }
        }
    }
}