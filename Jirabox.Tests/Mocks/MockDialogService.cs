using GalaSoft.MvvmLight.Threading;
using Jirabox.Core.Contracts;
using System;
using System.Windows;

namespace Jirabox.Tests.Mocks
{
    public class MockDialogService : IDialogService
    {
        public void ShowDialog(string message, string caption)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                MessageBox.Show(message, caption, MessageBoxButton.OK);
            });
        }

        public void SendErrorReportDialog(Exception e)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Phone.Controls.CustomMessageBox ShowCommentDialog(Model.Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
