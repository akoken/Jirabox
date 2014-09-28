using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.ViewModel
{
    public class LogWorkViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IJiraService jiraService;
        private readonly IDialogService dialogService;
        private CancellationTokenSource cancellationTokenSource = null;

        public RelayCommand LogWorkCommand { get; private set; }

        private bool isDataLoaded;
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

        public LogWorkViewModel(INavigationService navigationService, IJiraService jiraService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.jiraService = jiraService;
            this.dialogService = dialogService;

            LogWorkCommand = new RelayCommand(async()=>await LogWork());
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public void Initialize()
        {                      
            IsDataLoaded = true;
            cancellationTokenSource = new CancellationTokenSource();
        }

        private async Task LogWork()
        {
            IsDataLoaded = false;
            var issueKey = navigationService.GetNavigationParameter().ToString();
            if (IsPeriod)
            {
                while (IsStartDateLessThanEndDate(StartDate, EndDate))
                {                    
                    //2013-09-01T10:30:18.932+0530
                    var formattedDate = FormatDate(StartDate);                  
                    var timeSpent = String.Format("{0}h {1}m", Hour, minute);
                    var isLogged = await jiraService.LogWork(issueKey, formattedDate, timeSpent, Comment);
                    if (!isLogged)
                    {
                        dialogService.ShowDialog(AppResources.LogWorkError, AppResources.Error);
                        return;
                    }
                    StartDate = StartDate.AddDays(1);
                }
            }
            else
            {                
                var formattedDate = FormatDate(StartDate);
                var timeSpent = String.Format("{0}h {1}m", Hour, minute);
                var isLogged = await jiraService.LogWork(issueKey, formattedDate, timeSpent, Comment, cancellationTokenSource);
                if (!isLogged)
                {
                    dialogService.ShowDialog(AppResources.LogWorkError, AppResources.Error);
                    return;
                }
            }
            IsDataLoaded = true;
            dialogService.ShowDialog(AppResources.LogWorkSuccess, AppResources.Done);
            navigationService.GoBack();
        }

        public void Cancel()
        {
            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();
        }

        private string FormatDate(DateTime dateTime)
        {
            var day = dateTime.Day.ToString();
            var month = dateTime.Month.ToString();

            if(day.Length < 2)
            {
                day = String.Format("0{0}", day);
            }
            if(month.Length < 2)
            {
                month = String.Format("0{0}", month);
            }
            return String.Format("{0}-{1}-{2}T09:00:00.932+0530", dateTime.Year, month, day);
        }

        private bool IsStartDateLessThanEndDate(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year <= EndDate.Year && startDate.Month <= endDate.Month && startDate.Day <= endDate.Day)            
                return true;
            return false;
        }
    }
}
