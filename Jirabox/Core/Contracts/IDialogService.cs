
using Jirabox.Model;
using Microsoft.Phone.Controls;
using System;
namespace Jirabox.Core.Contracts
{
    public interface IDialogService
    {
        void ShowDialog(string message,string caption);

        void SendErrorReportDialog(Exception e);

        CustomMessageBox ShowCommentDialog(Comment comment, string caption);
    }
}
