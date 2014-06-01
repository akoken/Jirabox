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
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                var projectKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(projectKey);
            }
        }
    }
}