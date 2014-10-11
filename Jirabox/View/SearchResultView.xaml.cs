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
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                if (App.IsLoggedIn)
                {
                    if (NavigationContext.QueryString.ContainsKey("voiceCommandName"))
                    {
                        string voiceCommandName = NavigationContext.QueryString["voiceCommandName"];                       
                        switch (voiceCommandName)
                        {
                            case "Assigned":
                                vm.SetNavigationToAssignedIssues();
                                break;
                            case "Reported":
                                vm.SetNavigationToIssuesReportedByMe();
                                break;
                            case "Search":
                                {
                                    string searchText = NavigationContext.QueryString["dictatedSearchTerms"];                                    
                                    vm.SetNavigationSearchText(searchText);
                                }break;
                            default:
                                break;
                        }
                    }

                    vm.CleanUp();
                    vm.Initialize();
                }
                else
                {
                    vm.NavigateToLoginView();
                }
            }
            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as SearchResultViewModel;
            vm.CancelSearch();
        }
    }
}