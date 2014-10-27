using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class LogWorkView : PhoneApplicationPage
    {
        public LogWorkView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var vm = DataContext as LogWorkViewModel;
                vm.Initialize();                
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as LogWorkViewModel;
            vm.Cancel();
        }
    }
}