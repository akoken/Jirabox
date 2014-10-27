using Jirabox.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Jirabox.Views
{
    public partial class ProjectListView : PhoneApplicationPage
    {
        public ProjectListView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            projectList.SelectedItem = null;           
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var vm = this.DataContext as ProjectListViewModel;

            if (App.IsLoggedIn)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    await vm.InitializeData(true);
                    vm.RemoveBackEntry();
                }
            }
            else
            {
                vm.NavigateToLoginView();
            }
        }

        private void SearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextbox.Text == "")
            {
                ImageBrush watermark = new ImageBrush();
                watermark.ImageSource = new BitmapImage(new Uri(@"/Assets/search.dark.png", UriKind.Relative));
                watermark.AlignmentX = AlignmentX.Left;
                watermark.Stretch = Stretch.None;
                watermark.Opacity = .75;
                SearchTextbox.Background = watermark;
            }
            else
            {
                SearchTextbox.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void SearchTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchTextbox.Background.Opacity = .75;
        }
    }
}