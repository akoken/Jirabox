using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;

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

        public ChangeStatusViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.jiraService = jiraService;
            this.dialogService = dialogService;

            ChangeStatusCommand = new RelayCommand(async() =>
            {
                var isSuccess = await jiraService.PerformTransition(SelectedIssue.ProxyKey, SelectedTransition.Id, Comment);
                if (isSuccess)
                {
                    dialogService.ShowDialog("Status has been updated.", "Done");
                    GoBack();
                }
                else
                {
                    dialogService.ShowDialog("Opps! Something went wrong while changing status.", "Error");
                }
            });

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
    }
}
