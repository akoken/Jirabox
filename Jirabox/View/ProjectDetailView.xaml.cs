using Jirabox.Common;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class ProjectDetailView : PhoneApplicationPage
    {
        public ProjectDetailView()
        {
            InitializeComponent();                       
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as ProjectDetailViewModel;
            string projectKey = string.Empty;
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                projectKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(projectKey);
            }
            else if (NavigationContext.QueryString.ContainsKey("Key"))
            {
                var credential = StorageHelper.GetUserCredential();                
                App.ServerUrl = credential.ServerUrl;
                App.UserName = credential.UserName;
                App.Password = credential.Password;
                App.BaseUrl = string.Format("{0}/rest/api/latest/", App.ServerUrl);
                projectKey = projectKey = NavigationContext.QueryString["Key"];
                vm.CleanUp();
                vm.Initialize(projectKey, true);
            }
            else
                return;
          
        }
    }
}