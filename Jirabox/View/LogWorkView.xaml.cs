using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Jirabox.View
{
    public partial class LogWorkView : PhoneApplicationPage
    {
        public LogWorkView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var vm = DataContext as LogWorkViewModel;
                vm.Initialize();                
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            var vm = DataContext as LogWorkViewModel;
            vm.Cancel();
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