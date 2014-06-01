using Microsoft.Phone.Controls;

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
    }
}