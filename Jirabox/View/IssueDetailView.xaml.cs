using Jirabox.Model;
using Jirabox.Services;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;

namespace Jirabox.View
{
    public partial class IssueDetailView : PhoneApplicationPage
    {
        public IssueDetailView()
        {
            InitializeComponent();
            CommentList.SelectionChanged += CommentList_SelectionChanged;

            Messenger.Default.Register<bool>(this, "TaskBarVisibility", message =>
            {
                SystemTray.IsVisible = message;              
            });
        }

        void CommentList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedComment = CommentList.SelectedItem as Comment;
            if (selectedComment == null)
                return;

            Messenger.Default.Send(false, "TaskBarVisibility");

            var dialogService = new DialogService();            
            var messageBox = dialogService.ShowCommentDialog(selectedComment);
            messageBox.Dismissed += messageBox_Dismissed;
            CommentList.SelectedItem = null;            
        }

        void messageBox_Dismissed(object sender, DismissedEventArgs e)
        {
            Messenger.Default.Send(true, "TaskBarVisibility");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as IssueDetailViewModel;
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                var issueKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(issueKey);           
            }
        }
    }
}