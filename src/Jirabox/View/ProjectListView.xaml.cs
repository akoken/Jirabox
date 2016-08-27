using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.Views
{
    public partial class ProjectListView : PhoneApplicationPage
    {
        public ProjectListView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ProjectList.SelectedItem = null;           
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as ProjectListViewModel;

            if (App.IsLoggedIn)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    await vm.InitializeData(true);
                    vm.RemoveBackEntry();
                }
            }
            else
            {
                vm.NavigateToLoginView();
            }
        }     
    }
}