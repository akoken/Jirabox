using System.Collections.ObjectModel;

namespace Jirabox.Model
{
    public class StatusPackage
    {
        public ObservableCollection<Transition> Transitions { get; set; }

        public Issue SelectedIssue { get; set; }

        public SearchParameter SearchParameter { get; set; }
    }
}
