
namespace Jirabox.Model
{
    public class SearchParameter
    {
        public string SearchText { get; set; }

        public bool IsAssignedToMe { get; set; }

        public bool IsReportedByMe { get; set; }

        public bool IsFavourite { get; set; }

        public string JQL { get; set; }
    }
}
