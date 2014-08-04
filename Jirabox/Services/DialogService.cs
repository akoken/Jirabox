using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Resources;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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


        public CustomMessageBox ShowCommentDialog(Model.Comment comment, string caption)
        {
            var messageBox = new CustomMessageBox();
            var imageBrush = new ImageBrush();
            var scrollViewer = new ScrollViewer();
            imageBrush.ImageSource = StorageHelper.GetDisplayPicture(comment.Author.UserName);

            var border = new Border
            {
                Background = imageBrush,
                CornerRadius = new CornerRadius(5),
                Width = 48,
                Height = 48,
                Margin = new Thickness(3)
            };

            var txtDisplayName = new TextBlock { Text = comment.Author.DisplayName};
            txtDisplayName.VerticalAlignment = VerticalAlignment.Center;
            txtDisplayName.FontSize = 24;
            txtDisplayName.FontWeight = FontWeights.SemiBold;
            txtDisplayName.Margin = new Thickness(10, 0, 0, 0);

            var txtComment = new TextBlock { Text = comment.Message, TextWrapping = TextWrapping.Wrap };
            txtComment.Margin = new Thickness(15, 15, 10, 0);
            
            scrollViewer.Content = txtComment;
            var userInfoPanel = new StackPanel();
            userInfoPanel.Margin = new Thickness(10, 0, 0, 0);
            userInfoPanel.Orientation = Orientation.Horizontal;
            userInfoPanel.Children.Add(border);
            userInfoPanel.Children.Add(txtDisplayName);

            var rootPanel = new StackPanel();
            rootPanel.Margin = new Thickness(10, 30, 0, 0);
            rootPanel.Children.Add(userInfoPanel);
            rootPanel.Children.Add(scrollViewer);

            messageBox.Caption = caption;
            messageBox.Content = rootPanel;
            messageBox.LeftButtonContent = "OK";
            messageBox.Show();
            return messageBox;
        }
    }
}
