using Newtonsoft.Json;

namespace Jirabox.Model
{
    public class CreateIssueFields
    {
        public CreateIssueFields()
        {
            CreateIssueProject = new CreateIssueProject();
            IssueType = new CreateIssueType();
            Priority = new Priority();
        }

        [JsonProperty("project")]
        public CreateIssueProject CreateIssueProject { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
      
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("issuetype")]
        public CreateIssueType IssueType { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }
    }
}
