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
            SelectedIssue = parameter.SelectedIssue;
            Transitions = parameter.Transitions;
        }        

        public void GoBack()
        {
            navigationService.NavigationParameter = ((StatusPackage)navigationService.GetNavigationParameter()).SearchParameter;
            SelectedTransitionIndex = 0;
            Comment = string.Empty;    
            navigationService.GoBack();
        }

        private async Task ChangeStatus()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                dialogService.ShowDialog(AppResources.AddCommentMessage, AppResources.Warning);
                return;
            }

            IsDataLoaded = false;
            var isSuccess = await jiraService.PerformTransition(SelectedIssue.ProxyKey, Transitions[SelectedTransitionIndex].Id, Comment??string.Empty);
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
