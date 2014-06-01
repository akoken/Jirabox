using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Resources;
using Microsoft.Phone.Tasks;
using System;
using System.Reflection;

namespace Jirabox.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private string version;

        public RelayCommand EmailCommand{ get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand ReviewCommand { get; private set; }
        public string Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }

        public AboutViewModel()
        {
            EmailCommand = new RelayCommand(SendEmail);
            ShareCommand = new RelayCommand(ShareThisApp);
            ReviewCommand = new RelayCommand(ReviewThisApp);

            var app = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            Version = app.Version.ToString();
        }
        private void SendEmail()
        {
            var emailComposeTask = new EmailComposeTask();         
            emailComposeTask.To = AppResources.ApplicationEmail;            
            emailComposeTask.Show();
        }
        private void ShareThisApp()
        {
            var shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = AppResources.ApplicationTitle;
            shareLinkTask.LinkUri = new Uri("http://www.windowsphone.com/s?appid=d1413cdb-ea8f-4fba-b3b5-0d776f69b8bb", UriKind.Absolute);
            shareLinkTask.Message = AppResources.ApplicationDescription;
            shareLinkTask.Show();
        }
        private void ReviewThisApp()
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

    }
}
