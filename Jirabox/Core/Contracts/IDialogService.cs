
using Jirabox.Model;
using Microsoft.Phone.Controls;
namespace Jirabox.Core.Contracts
{
    public interface IDialogService
    {
        void ShowDialog(string message,string caption);      

        CustomMessageBox ShowCommentDialog(Comment comment);

        CustomMessageBox ShowPromptDialog(string warningMessage, string confirmMessage, string caption);
    }
}
