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
        private string comment;
        private string issueKey;

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

        public string IssueKey
        {
            get
            {
                return issueKey;
            }
            set
            {
                if (issueKey != value)
                {
                    issueKey = value;
                    RaisePropertyChanged(() => IssueKey);
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

            ChangeStatusCommand = new RelayCommand(() =>
            {
                jiraService.PerformTransition(IssueKey, SelectedTransition.Id, Comment);
            });

            CancelCommand = new RelayCommand(() =>
            {
                navigationService.NavigationParameter = ((StatusPackage)navigationService.GetNavigationParameter()).SearchParameter;
                navigationService.GoBack();
            });
        }

        public void Initialize()
        {
            var parameter = navigationService.GetNavigationParameter() as StatusPackage;
            IssueKey = parameter.IssueKey;
            Transitions = parameter.Transitions;
        }
    }
}
