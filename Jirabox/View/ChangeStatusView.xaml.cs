using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class ChangeStatusView : PhoneApplicationPage
    {
        public ChangeStatusView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as ChangeStatusViewModel;

           // if(TransitionPicker.ItemsSource == null)
            vm.Initialize();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as ChangeStatusViewModel;
            vm.GoBack();
        }
    }
}