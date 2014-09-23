using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using System;
using System.Threading.Tasks;

namespace Jirabox.ViewModel
{
    public class LogWorkViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IJiraService jiraService;

        public RelayCommand LogWorkCommand { get; private set; }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    RaisePropertyChanged(() => StartDate);
                }
            }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set 
            {
                if (endDate != value)
                {
                    endDate = value;
                    RaisePropertyChanged(() => EndDate);
                }
            }
        }


        private int hour;

        public int Hour
        {
            get { return hour; }
            set 
            {
                if (hour != value)
                {
                    hour = value;
                    RaisePropertyChanged(() => Hour);
                }
            }
        }

        private int minute;

        public int Minute
        {
            get { return minute; }
            set 
            {
                if (minute != value)
                {
                    minute = value;
                    RaisePropertyChanged(() => Minute);
                }
            }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set 
            {
                if (comment != value)
                {
                    comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        private bool isPeriod;

        public bool IsPeriod
        {
            get { return isPeriod; }
            set 
            {
                if (isPeriod != value)
                {
                    isPeriod = value;
                    RaisePropertyChanged(() => IsPeriod);
                }
            }
        }


        public LogWorkViewModel(INavigationService navigationService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.jiraService = jiraService;

            LogWorkCommand = new RelayCommand(async()=>await LogWork());
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        private async Task LogWork()
        {
            var issueKey = navigationService.GetNavigationParameter().ToString();
            while (StartDate.ToShortDateString() != EndDate.ToShortDateString())
            {
                var date = String.Format("{0:s}", StartDate);
                var timeSpent = String.Format("{0}h {1}m", Hour, minute);
                var result = await jiraService.LogWork(issueKey, date, timeSpent, Comment);
                StartDate = StartDate.AddDays(1);
            }            
        }
    }
}
