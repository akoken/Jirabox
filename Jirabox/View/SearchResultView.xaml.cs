using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class SearchResultView : PhoneApplicationPage
    {
        public SearchResultView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as SearchResultViewModel;
            vm.CleanUp();
            vm.Initialize();
        }
    }
}