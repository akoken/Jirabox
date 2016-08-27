using System.ComponentModel;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Jirabox.View
{
    public partial class LogWorkView : PhoneApplicationPage
    {
        public LogWorkView()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, "TaskBarVisibility", message =>
            {
                SystemTray.IsVisible = message;
                ProgressIndicator.IsVisible = message;
            });
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