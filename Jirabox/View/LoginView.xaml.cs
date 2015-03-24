using System.ComponentModel;
using System.Windows.Navigation;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;

namespace Jirabox.View
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.CancelLogin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.RemoveBackEntry();
        }      
    }
}