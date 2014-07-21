using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.Resources;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Jirabox.ViewModel
{
    public class ChangeStatusViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;
        private readonly IJiraService jiraService;

        private ObservableCollection<Transition> transitions;
        private Transition selectedTransition;
        private Issue selectedIssue;
        private bool isDataLoaded;
        private string comment;

        public RelayCommand ChangeStatusCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }


        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        public Issue SelectedIssue
        {
            get
            {
                return selectedIssue;
            }
            set
            {
                if (selectedIssue != value)
                {
                    selectedIssue = value;
                    RaisePropertyChanged(() => SelectedIssue);
                }
            }
        }

        public Transition SelectedTransition
        {
            get
            {
                return selectedTransition;
            }
            set
            {
                if (selectedTransition != value)
                {
                    selectedTransition = value;
                    RaisePropertyChanged(() => SelectedTransition);
                }
            }
        }
        public ObservableCollection<Transition> Transitions
        {
            get
            {
                return transitions;
            }
            set
            {
                if (transitions != value)
                {
                    transitions = value;
                    RaisePropertyChanged(() => Transitions);
                }
            }
        }

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

        public ChangeStatusViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.jiraService = jiraService;
            this.dialogService = dialogService;

            ChangeStatusCommand = new RelayCommand(async() => await ChangeStatus());
            CancelCommand = new RelayCommand(GoBack);           
        }

        public void Initialize()
        {
            var parameter = navigationService.GetNavigationParameter() as StatusPackage;
            SelectedIssue = parameter.SelectedIssue;
            Transitions = parameter.Transitions;
        }        

        public void GoBack()
        {
            navigationService.NavigationParameter = ((StatusPackage)navigationService.GetNavigationParameter()).SearchParameter;                  
            navigationService.GoBack();
        }

        private async Task ChangeStatus()
        {
            IsDataLoaded = false;
            var isSuccess = await jiraService.PerformTransition(SelectedIssue.ProxyKey, SelectedTransition.Id, Comment);
            if (isSuccess)
            {
                dialogService.ShowDialog(AppResources.StatusUpdatedMessage, AppResources.Done);
                GoBack();
            }
            else
            {
                dialogService.ShowDialog(AppResources.StatusUpdateErrorMessage, AppResources.Error);
            }
            IsDataLoaded = true;
        }
    }
}
