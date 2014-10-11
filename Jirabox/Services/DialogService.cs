using Jirabox.Common;
using Jirabox.Core.Contracts;
using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Jirabox.Services
{
    public class DialogService : IDialogService
    {
        public void ShowDialog(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }     

        public CustomMessageBox ShowCommentDialog(Model.Comment comment)
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
            
            messageBox.Content = rootPanel;
            messageBox.LeftButtonContent = "OK";
            messageBox.Show();
            return messageBox;
        }

        public CustomMessageBox ShowPromptDialog(string warningMessage, string confirmMessage, string caption)
        {
            var messageBox = new CustomMessageBox();
            var warningTextBlock = new TextBlock { Text = warningMessage, TextWrapping = System.Windows.TextWrapping.Wrap };
            warningTextBlock.Margin = new Thickness(15, 15, 0, 0);

            var confirmTextBlock = new TextBlock { Text = confirmMessage, TextWrapping = System.Windows.TextWrapping.Wrap };
            confirmTextBlock.Margin = new Thickness(15, 15, 0, 0);

            var rootPanel = new StackPanel();
            rootPanel.Children.Add(warningTextBlock);
            rootPanel.Children.Add(confirmTextBlock);

            messageBox.Caption = caption;
            messageBox.Content = rootPanel;
            messageBox.LeftButtonContent = "OK";
            messageBox.RightButtonContent = "Cancel";
            messageBox.Show();
            return messageBox;
        }
    }
}
