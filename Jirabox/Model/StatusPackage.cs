using System.Collections.ObjectModel;

namespace Jirabox.Model
{
    public class StatusPackage
    {
        public ObservableCollection<Transition> Transitions { get; set; }

        public string IssueKey { get; set; }

        public SearchParameter SearchParameter { get; set; }
    }
}
