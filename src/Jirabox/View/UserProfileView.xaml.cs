using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class UserProfileView : PhoneApplicationPage
    {
        public UserProfileView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as UserProfileViewModel;
            vm.Initialize();
        }
    }
}