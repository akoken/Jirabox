
using System;
namespace Jirabox.Core.Contracts
{
    public interface IDialogService
    {
        void ShowDialog(string message,string caption);

        void SendErrorReportDialog(Exception e);
    }
}
