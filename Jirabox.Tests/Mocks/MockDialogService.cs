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
            throw new NotImplementedException();
        }

        public Microsoft.Phone.Controls.CustomMessageBox ShowCommentDialog(Model.Comment comment)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Phone.Controls.CustomMessageBox ShowPromptDialog(string warningMessage, string confirmMessage, string caption)
        {
            throw new NotImplementedException();
        }
    }
}
