using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class IssueDetailView : PhoneApplicationPage
    {
        public IssueDetailView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as IssueDetailViewModel;
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                var issueKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(issueKey);
                vm.DynamicMargin = new Thickness(0, Application.Current.Host.Content.ActualHeight, 0, 0);
            }
        }
    }
}