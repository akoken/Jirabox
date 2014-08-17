using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;

namespace Jirabox.View
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.CancelLogin();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            vm.RemoveBackEntry();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Windows Phone does not support PropertyChanged syntax in binding
            var textBox = sender as TextBox;

            // Update the binding source
            BindingExpression bindingExpr = textBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            //Windows Phone does not support PropertyChanged syntax in binding
            var passwordBox = sender as PasswordBox;

            // Update the binding source
            BindingExpression bindingExpr = passwordBox.GetBindingExpression(PasswordBox.PasswordProperty);
            bindingExpr.UpdateSource();
        }
    }
}