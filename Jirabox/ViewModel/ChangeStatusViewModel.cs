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
        private int selectedTransitionIndex;
        private Issue selectedIssue;
        private bool isDataLoaded;
        private bool isTaskbarVisible = true;

        public RelayCommand ChangeStatusCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }      

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

        public bool IsTaskbarVisible
        {
            get { return isTaskbarVisible; }
            set
            {
                if (isTaskbarVisible != value)
                {
                    isTaskbarVisible = value;
                    RaisePropertyChanged(() => IsTaskbarVisible);
                }
            }
        }

        public int SelectedTransitionIndex
        {
            get
            {
                return selectedTransitionIndex;
            }
            set
            {
                if (selectedTransitionIndex != value)
                {
                    selectedTransitionIndex = value;
                    RaisePropertyChanged(() => SelectedTransitionIndex);
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
            IsDataLoaded = true;
            var parameter = navigationService.GetNavigationParameter() as StatusPackage;

            if (parameter == null)
            {
                dialogService.ShowDialog(AppResources.NavigationParameterIsNullError, AppResources.Error);
                return;
            }
            SelectedIssue = parameter.SelectedIssue;
            Transitions = parameter.Transitions;
        }        

        public void GoBack()
        {
            var navigationPackage = navigationService.GetNavigationParameter();
            if (navigationPackage != null)
                navigationService.NavigationParameter = ((StatusPackage)navigationPackage).SearchParameter;
                            
            SelectedTransitionIndex = 0;            
            navigationService.GoBack();
        }

        private async Task ChangeStatus()
        {
           
            IsDataLoaded = false;
            var isSuccess = await jiraService.PerformTransition(SelectedIssue.ProxyKey, Transitions[SelectedTransitionIndex].Id);
            IsDataLoaded = true;
            IsTaskbarVisible = false;

            if (isSuccess)
            {
                
                dialogService.ShowDialog(AppResources.StatusUpdatedMessage, AppResources.Done);
                IsTaskbarVisible = true;
                GoBack();
            }
            else
            {
                IsTaskbarVisible = false;
                dialogService.ShowDialog(AppResources.StatusUpdateErrorMessage, AppResources.Error);
                IsTaskbarVisible = true;
            }                      
        }       
    }
}
