using Jirabox.ViewModel;
using Microsoft.Phone.Controls;

namespace Jirabox.View
{
    public partial class SettingsView : PhoneApplicationPage
    {
        public SettingsView()
        {
            InitializeComponent();            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as SettingsViewModel;
            var maxSearchResult = SearchResultCount.Text;
            vm.SaveMaxSearchResultCommand.Execute(int.Parse(maxSearchResult));            
        }
    }
}