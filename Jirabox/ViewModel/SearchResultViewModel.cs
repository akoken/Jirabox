using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;
using System.Threading;

namespace Jirabox.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private IJiraService jiraService;
        private INavigationService navigationService;
        private bool isDataLoaded;
        private CancellationTokenSource cancellationTokenSource = null;

        public RelayCommand<Issue> ShowIssueDetailCommand { get; private set; }

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                if (isDataLoaded != value)
                {
                    isDataLoaded = value;
                    RaisePropertyChanged(() => IsDataLoaded);
                }
            }
        }

        private ObservableCollection<Issue> issues;

        public ObservableCollection<Issue> Issues
        {
            get { return issues; }
            set
            {
                if (issues != value)
                {
                    issues = value;
                    RaisePropertyChanged(() => Issues);
                }
            }
        }       

        public SearchResultViewModel(IJiraService jiraService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.navigationService = navigationService;
            ShowIssueDetailCommand = new RelayCommand<Issue>(issue => NavigateToIssueDetailView(issue), issue => issue != null);
        }

        public async void Initialize()
        {
            IsDataLoaded = false;
            var searchParameter = (SearchParameter)navigationService.GetNavigationParameter();
            if (searchParameter != null)
            {
                cancellationTokenSource = new CancellationTokenSource();
                Issues = await jiraService.Search(searchParameter.SearchText, searchParameter.IsAssignedToMe, searchParameter.IsReportedByMe, cancellationTokenSource);
            }
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            Issues = null;
        }
        public void CancelSearch()
        {
            if(cancellationTokenSource != null)
            cancellationTokenSource.Cancel();
        }

        private void NavigateToIssueDetailView(Issue selectedIssue)
        {
            navigationService.Navigate<IssueDetailViewModel>(selectedIssue.ProxyKey);
        }

    }
}
