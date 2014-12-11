using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;

namespace Jirabox.View
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.CancelLogin();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.RemoveBackEntry();
        }      
    }
}