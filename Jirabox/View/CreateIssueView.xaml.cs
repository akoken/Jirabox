using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class CreateIssueView : PhoneApplicationPage
    {
        public CreateIssueView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as CreateIssueViewModel;           

            //Prevent getting data twice
            if (vm.Project != null) return;
            vm.Initialize();      
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = this.DataContext as CreateIssueViewModel;
            vm.CleanUp();
        }      
    }
}