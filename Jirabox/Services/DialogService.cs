using Jirabox.Core.Contracts;
using Jirabox.Resources;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;

namespace Jirabox.Services
{
    public class DialogService : IDialogService
    {
        public void ShowDialog(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        public void SendErrorReportDialog(Exception e)
        {
            var errorMessageBox = new CustomMessageBox();
            errorMessageBox.Caption = "Error";
            errorMessageBox.Message = AppResources.UnhandledErrorMessage;
            errorMessageBox.LeftButtonContent = "Yes";
            errorMessageBox.RightButtonContent = "No";
            errorMessageBox.Dismissed += (s1, e1) =>
            {
                if (e1.Result == CustomMessageBoxResult.LeftButton)
                {
                    //Send error report
                    var emailComposeTask = new EmailComposeTask();
                    emailComposeTask.To = AppResources.ApplicationEmail;
                    emailComposeTask.Subject = AppResources.ErrorReportMailSubject;
                    emailComposeTask.Body = string.Format(AppResources.ErrorReportMailBody, e.Message, System.Environment.NewLine, e.StackTrace);
                    emailComposeTask.Show();
                }
            };
            errorMessageBox.Show();            
        }
    }
}
