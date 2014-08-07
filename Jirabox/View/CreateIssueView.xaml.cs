using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class CreateIssueView : PhoneApplicationPage
    {
        private readonly CreateIssueViewModel vm;
        public CreateIssueView()
        {
            InitializeComponent();
            vm = this.DataContext as CreateIssueViewModel;     
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {                   
            //Prevent getting data twice
            if (vm.Project != null) return;
            vm.Initialize();      
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {            
            vm.CleanUp();
        }

        private void ListPickerIssueTypes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {         
            vm.SelectedIssueTypeIndex = ListPickerIssueTypes.SelectedIndex;
        }

        private void ListPickerPriorities_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {            
            vm.SelectedPriorityIndex = ListPickerPriorities.SelectedIndex;
        }
    }
}