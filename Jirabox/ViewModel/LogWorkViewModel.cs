using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core;
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

        private CancellationTokenSource cancellationTokenSource;
        private bool isTaskbarVisible = true;

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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public LogWorkViewModel(INavigationService navigationService, IJiraService jiraService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.jiraService = jiraService;
            this.dialogService = dialogService;    
            LogWorkCommand = new RelayCommand(async () => await LogWork());       
        }

        public void Initialize()
        {                      
            IsDataLoaded = true;
            cancellationTokenSource = new CancellationTokenSource();
           
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Comment = string.Empty;
            IsPeriod = false;
            Hour = default(int);
            Minute = default(int);
        }

        private async Task LogWork()
        {
            var issueKey = navigationService.GetNavigationParameter().ToString();
            
            OperationResult validationResult = ValidateInputs(issueKey);
            if (!validationResult.IsValid)
            {
                IsTaskbarVisible = false;
                dialogService.ShowDialog(validationResult.ErrorMessage, AppResources.Error);
                IsTaskbarVisible = true;
                return;
            }

            if (IsPeriod)
            {
                while (IsFirstDateLessOrEqualThanSecondDate(StartDate, EndDate))
                {                                        
                    var formattedDate = FormatDate(StartDate);                  
                    var timeSpent = String.Format("{0}h {1}m", Hour, minute);
                    IsDataLoaded = false;
                    var isLogged = await jiraService.LogWork(issueKey, formattedDate, timeSpent, Comment);
                    IsDataLoaded = true;

                    if (!isLogged)
                    {
                        IsTaskbarVisible = false;
                        dialogService.ShowDialog(AppResources.LogWorkError, AppResources.Error);
                        IsTaskbarVisible = true;
                        return;
                    }
                    StartDate = StartDate.AddDays(1);
                }
            }
            else
            {                
                var formattedDate = FormatDate(StartDate);
                var timeSpent = String.Format("{0}h {1}m", Hour, minute);
                IsDataLoaded = false;
                var isLogged = await jiraService.LogWork(issueKey, formattedDate, timeSpent, Comment, cancellationTokenSource);
                IsDataLoaded = true;
                if (!isLogged)
                {
                    IsTaskbarVisible = false;
                    dialogService.ShowDialog(AppResources.LogWorkError, AppResources.Error);
                    IsTaskbarVisible = true;
                    return;
                }
            }
            IsTaskbarVisible = false;
            dialogService.ShowDialog(AppResources.LogWorkSuccess, AppResources.Done);
            IsTaskbarVisible = true;
            navigationService.GoBack();
        }

        public void Cancel()
        {
            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();
        }

        private string FormatDate(DateTime dateTime)
        {          
            var timeOffset = new DateTimeOffset(DateTime.Now).Offset;
            var offsetStr = timeOffset.ToString();
// ReSharper disable once StringLastIndexOfIsCultureSpecific.1
            string formattedTimeOffset = offsetStr.Remove(offsetStr.LastIndexOf(":"), 3);

            if (timeOffset.Hours > -1)
                formattedTimeOffset = "+" + formattedTimeOffset;
                        
            return String.Format("{0}-{1}-{2}T00:00:00.000{3}", dateTime.Year, dateTime.Month.ToString("00"), dateTime.Day.ToString("00"), formattedTimeOffset.Replace(":",""));
        }      

        private OperationResult ValidateInputs(string issueKey)
        {
            if (String.IsNullOrEmpty(issueKey))
            {
                return new OperationResult { IsValid = false, ErrorMessage = AppResources.IssueKeyNotFoundMessage };
            }

            if (Hour == 0 && Minute == 0) return new OperationResult { IsValid = false, ErrorMessage = AppResources.LogWorkLoggedTimeZeroErrorMessage };

            if (IsPeriod)
            {
                if (!IsFirstDateLessOrEqualThanSecondDate(EndDate, DateTime.Today)) return new OperationResult { IsValid = false, ErrorMessage = AppResources.LogWorkEndDateGreaterThanTodayErrorMessage };
                if (!IsFirstDateLessOrEqualThanSecondDate(StartDate, EndDate)) return new OperationResult { IsValid = false, ErrorMessage = AppResources.LogWorkStartDateGreaterThanEndDateErrorMessage };
            }
            else
            {
                if (!IsFirstDateLessOrEqualThanSecondDate(StartDate, DateTime.Today)) return new OperationResult { IsValid = false, ErrorMessage = AppResources.LogWorkStartDateGreaterThanTodayErrorMessage };
            }

            return new OperationResult { IsValid = true };
        }

        private bool IsFirstDateLessOrEqualThanSecondDate(DateTime firstDate, DateTime secondDate)
        {
            var result = firstDate.Date.CompareTo(secondDate.Date);
            if (result <= 0)
                return true;
            return false;
        }
    }
}
