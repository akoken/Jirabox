using Jirabox.Model;
using Jirabox.Services;
using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class IssueDetailView : PhoneApplicationPage
    {
        public IssueDetailView()
        {
            InitializeComponent();
            CommentList.SelectionChanged += CommentList_SelectionChanged;
        }

        void CommentList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedComment = CommentList.SelectedItem as Comment;
            if (selectedComment == null)
                return;

            var dialogService = new DialogService();
            SystemTray.IsVisible = false;
            dialogService.ShowCommentDialog(selectedComment);
            CommentList.SelectedItem = null;
            SystemTray.IsVisible = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as IssueDetailViewModel;
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                var issueKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(issueKey);           
            }
        }
    }
}