using System.ComponentModel;
using System.Windows.Navigation;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;

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

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            var vm = DataContext as LogWorkViewModel;
            vm.Cancel();
        }
    }
}