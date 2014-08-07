using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class AddCommentView : PhoneApplicationPage
    {
        public AddCommentView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as AddCommentViewModel;
            if (NavigationContext.QueryString.ContainsKey("param"))
            {
                var issueKey = NavigationContext.QueryString["param"];
                vm.CleanUp();
                vm.Initialize(issueKey);                
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Windows Phone does not support PropertyChanged syntax in binding
            var textBox = sender as TextBox;
            // Update the binding source
            BindingExpression bindingExpr = textBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
        }
    }
}